using DA.Messaging.Requests.Abstractions;
using DA.Results;

namespace DA.Messaging.Requests.Queries;

/// <summary>
/// Handler for an <see cref="IQuery{TResponse}"/>
/// </summary>
/// <typeparam name="TRequest">The type of the request to handle.</typeparam>
/// <typeparam name="TResponse">The response that is expected given the request.</typeparam>
public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IQuery<TResponse>;
