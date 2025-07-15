using DA.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace DA.Messaging;

/// <inheritdoc cref="IRequestDispatcher" />
internal sealed class RequestDispatcher(IServiceProvider _provider, ILogger<RequestDispatcher> _logger) : IRequestDispatcher
{
    // Caches: requestType => delegate (handler, request, cancellationToken) => Task<object>
    private static readonly ConcurrentDictionary<Type, Func<object, object, CancellationToken, Task<object>>> _cache = new();

    /// <inheritdoc cref="IRequestDispatcher.DispatchAsync{TResponse}"/>
    public async Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var requestType = request.GetType();

        var handlerDelegate = _cache.GetOrAdd(requestType, CreateHandlerDelegate<TResponse>);
        var handler = _provider.GetService(GetHandlerType<TResponse>(requestType)) ?? throw new InvalidOperationException($"No handler registered for {requestType.Name}");

        _logger.LogDebug("Dispatching {RequestType} via {HandlerType}", requestType.Name, handler.GetType().Name);

        var result = await handlerDelegate(handler, request, cancellationToken);
        return (TResponse)result;
    }

    private static Type GetHandlerType<TResponse>(Type requestType) =>
        typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

    private static Func<object, object, CancellationToken, Task<object>> CreateHandlerDelegate<TResponse>(Type requestType)
    {
        var handlerType = GetHandlerType<TResponse>(requestType);
        var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync))!;

        // (object handler, object request, CancellationToken ct) => ((IRequestHandler<TRequest, TResponse>)handler).HandleAsync((TRequest)request, ct)
        var handlerParam = Expression.Parameter(typeof(object), "handler");
        var requestParam = Expression.Parameter(typeof(object), "request");
        var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

        var castedHandler = Expression.Convert(handlerParam, handlerType);
        var castedRequest = Expression.Convert(requestParam, requestType);

        var call = Expression.Call(castedHandler, method, castedRequest, ctParam);

        var lambda = Expression.Lambda<Func<object, object, CancellationToken, Task<object>>>(
            Expression.Call(typeof(RequestDispatcher), nameof(WrapTask), [typeof(TResponse)], call),
            handlerParam, requestParam, ctParam
        );

        return lambda.Compile();
    }

    private static async Task<object> WrapTask<T>(Task<T> task) => (await task)!;
}
