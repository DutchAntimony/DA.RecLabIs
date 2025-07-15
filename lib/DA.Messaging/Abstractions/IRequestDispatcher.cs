namespace DA.Messaging.Abstractions;

/// <summary>
/// Dispatcher that handles a <see cref="IRequest{TResponse}"/> and returns a response of type <typeparamref name="TResponse"/>.
/// </summary>
public interface IRequestDispatcher
{
    /// <summary>
    /// Dispatches a request to the appropriate handler and returns the response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request that should be handled.</param>
    /// <param name="cancellationToken">Optional: token to cancel the async handling of the request.</param>
    /// <returns>The <typeparamref name="TResponse"/> that is a result of handling the request.</returns>
    Task<TResponse> DispatchAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
