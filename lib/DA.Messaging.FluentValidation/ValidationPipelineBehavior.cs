using DA.Messaging.Requests.Abstractions;
using DA.Messaging.Requests.Behaviours;
using DA.Results;
using DA.Results.Errors;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace DA.Messaging.Validation;

/// <summary>
/// Validation behaviour that validates an incoming request using the configured validators.
/// It will return a <see cref="Result"/> or <see cref="Result{TResponse}"/> with a <see cref="ValidationError"/> if validation fails.
/// </summary>
/// <typeparam name="TRequest">The type of the request</typeparam>
/// <typeparam name="TResponse">The type of the response that matches the request</typeparam>
/// <param name="validators">Collection of <see cref="IValidator{TRequest}"/> that are registered via dependency injection.</param>
internal sealed class ValidationPipelineBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger)
    : IRequestPipelineBehaviour<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
{
    private static readonly ConcurrentDictionary<Type, Func<Error, object>> _failureFactoryCache = new();

    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        logger.LogDebug("Validating {RequestType}", typeof(TRequest).Name);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        var validationError = new ValidationError(failures.Select(ValidationFailureMap));
        logger.LogWarning("Validation failed for {RequestType}: {ErrorCount} error(s)", typeof(TRequest).Name, failures.Count);

        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)CreateFailureResult(typeof(Result), validationError)!;
        }

        if (IsGenericResultType(typeof(TResponse)))
        {
            return (TResponse)CreateFailureResult(typeof(TResponse), validationError)!;
        }

        throw new ValidationException($"Validation failed", failures);
    }

    private static bool IsGenericResultType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>);
    }

    private static object CreateFailureResult(Type resultType, Error error)
    {
        var factory = _failureFactoryCache.GetOrAdd(resultType, static type =>
        {
            var methodInfo = type.GetMethod("Failure", BindingFlags.Public | BindingFlags.Static, [typeof(Error)])
                ?? throw new InvalidOperationException($"Could not find static Failure(Error) method on {type}");

            var errorParam = Expression.Parameter(typeof(Error), "error");
            var call = Expression.Call(methodInfo, errorParam);
            var lambda = Expression.Lambda<Func<Error, object>>(Expression.Convert(call, typeof(object)), errorParam);
            return lambda.Compile();
        });

        return factory(error);
    }

    private ValidationFailure ValidationFailureMap(FluentValidation.Results.ValidationFailure failure)
    {
        return new ValidationFailure(failure.PropertyName, failure.ErrorMessage);
    }
}