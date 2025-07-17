using DA.Messaging.Requests.Abstractions;
using DA.Messaging.Requests.Behaviours;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace DA.Messaging.Requests;

/// <inheritdoc cref="IRequestDispatcher" />
internal sealed class RequestDispatcher(IServiceProvider _provider, ILogger<RequestDispatcher> _logger) : IRequestDispatcher
{
    // Caches: requestType => delegate (handler, request, cancellationToken) => Task<object>
    private static readonly ConcurrentDictionary<Type, Func<object, IServiceProvider, CancellationToken, Task<object>>> _cache = new();

    /// <inheritdoc cref="IRequestDispatcher.DispatchAsync{TResponse}"/>
    public async Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var requestType = request.GetType();

        var executorDelegate = _cache.GetOrAdd(requestType, CreateExecutorDelegate<TResponse>);
        var result = await executorDelegate(request, _provider, cancellationToken);

        return (TResponse)result;
    }

    private static Func<object, IServiceProvider, CancellationToken, Task<object>> CreateExecutorDelegate<TResponse>(Type requestType)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviorType = typeof(IRequestPipelineBehaviour<,>).MakeGenericType(requestType, typeof(TResponse));
        var executorType = typeof(RequestPipelineExecutor<,>).MakeGenericType(requestType, typeof(TResponse));

        // Parameters for the lambda
        var requestParam = Expression.Parameter(typeof(object), "request");
        var providerParam = Expression.Parameter(typeof(IServiceProvider), "provider");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        // Cast request
        var castedRequest = Expression.Convert(requestParam, requestType);

        // provider.GetServices<IRequestPipelineBehavior<TRequest, TResponse>>()
        var getBehaviorsCall = Expression.Call(
            typeof(ServiceProviderServiceExtensions),
            nameof(ServiceProviderServiceExtensions.GetServices),
            [behaviorType],
            providerParam);

        // provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>()
        var getHandlerCall = Expression.Call(
            typeof(ServiceProviderServiceExtensions),
            nameof(ServiceProviderServiceExtensions.GetRequiredService),
            [handlerType],
            providerParam);

        // new RequestPipelineExecutor<TRequest, TResponse>(behaviors, handler)
        var executorCtor = executorType.GetConstructor([typeof(IEnumerable<>).MakeGenericType(behaviorType), handlerType])!;
        var executorVar = Expression.Variable(executorType, "executor");
        var assignExecutor = Expression.Assign(
            executorVar,
            Expression.New(executorCtor, getBehaviorsCall, getHandlerCall));

        // executor.ExecuteAsync((TRequest)request, ct)
        var executeCall = Expression.Call(
            executorVar,
            nameof(RequestPipelineExecutor<IRequest<object>, object>.ExecuteAsync),
            Type.EmptyTypes,
            castedRequest, ctParam);

        // Wrap result
        var wrapCall = Expression.Call(
            typeof(RequestDispatcher),
            nameof(WrapTask),
            [typeof(TResponse)],
            executeCall);

        // Full block
        var body = Expression.Block(
            new[] { executorVar },
            assignExecutor,
            wrapCall);

        return Expression.Lambda<Func<object, IServiceProvider, CancellationToken, Task<object>>>(
            body, requestParam, providerParam, ctParam).Compile();
    }

    private static async Task<object> WrapTask<T>(Task<T> task) => (await task!)!;
}
