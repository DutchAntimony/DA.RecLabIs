using DA.Messaging.Requests.Abstractions;

namespace DA.Messaging.Requests.Behaviours;

/// <summary>
/// Executor for a request pipeline that chains together multiple behaviours and a request handler.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <param name="behaviours"></param>
/// <param name="handler"></param>
internal sealed class RequestPipelineExecutor<TRequest, TResponse>(
    IEnumerable<IRequestPipelineBehaviour<TRequest, TResponse>> behaviours,
    IRequestHandler<TRequest, TResponse> handler)
        where TRequest : class, IRequest<TResponse>
{
    
    public Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        RequestHandlerDelegate<TResponse> handlerDelegate = () => handler.HandleAsync(request, cancellationToken);

        foreach (var behaviour in behaviours.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => behaviour.HandleAsync(request, next, cancellationToken);
        }

        return handlerDelegate();
    }
}