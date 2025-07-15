using DA.Messaging.Pagination;
using DA.Messaging.Requests.Abstractions;
using DA.Results;

namespace DA.Messaging.Requests.Queries;

/// <summary>
/// Handler for an <see cref="IPaginatedQuery{TResponse}"/>
/// </summary>
/// <typeparam name="TRequest">The type of the request to handle.</typeparam>
/// <typeparam name="TResponse">The response that is expected given the request.</typeparam>
public interface IPaginatedQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<PaginatedCollection<TResponse>>>
    where TRequest : IPaginatedQuery<TResponse>;
