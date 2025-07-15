namespace DA.Messaging.Abstractions;

/// <summary>
/// Marker interface for every request that can be handled by the <see cref="IRequestDispatcher"/>
/// </summary>
/// <remarks>Use a more specific request, such as <see cref="IQuery{TResponse}"/> of <see cref="ICommand"/> where possible.</remarks>
/// <typeparam name="TResponse">The response that is returned after handling the request.</typeparam>
public interface IRequest<TResponse>;
