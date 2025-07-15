namespace DA.Messaging.Abstractions;

/// <summary>
/// Generic request handler for any request.
/// </summary>
/// <remarks>This interface is only for wiring purposes. 
/// Use the specific interface for the specific type of request where possible.</remarks>
/// <typeparam name="TRequest">The type of the request to handle.</typeparam>
/// <typeparam name="TResponse">The response that is expected given the request.</typeparam>
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
