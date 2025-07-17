using DA.Messaging.Requests.Abstractions;

namespace DA.Messaging.Requests.Behaviours;

/// <summary>
/// Interface for adding chained behaviour before and after a <typeparamref name="TRequest"/> 
/// is handled by a <see cref="IRequestHandler{TRequest, TResponse}"/>
/// </summary>
/// <typeparam name="TRequest">The type of the request</typeparam>
/// <typeparam name="TResponse">The type of the response that matches the request</typeparam>
public interface IRequestPipelineBehaviour<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Delegate voor het uitvoeren van een request.
/// </summary>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
