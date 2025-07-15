using DA.Messaging.Pagination;
using DA.Messaging.Requests.Abstractions;
using DA.Results;

namespace DA.Messaging.Requests.Queries;

/// <summary>
/// Request to query a collection of data that must be returned paged.
/// The handler will wrap the response in a <see cref="Result{PaginatedCollection{TResponse}}" /> object">
/// </summary>
/// <typeparam name="TResponse">The type of the data entry that is returned.</typeparam>
public interface IPaginatedQuery<TResponse> : IRequest<Result<PaginatedCollection<TResponse>>>
{
    /// <summary>
    /// Information about the pagesize and page number of the requested data.
    /// </summary>
    PagingInformation Paging { get; }
}
