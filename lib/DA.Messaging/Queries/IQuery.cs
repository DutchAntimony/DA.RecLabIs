using DA.Messaging.Abstractions;
using DA.Results;

namespace DA.Messaging.Queries;

/// <summary>
/// Request to query a specific data entry that is handled by an <see cref="IQueryHandler{TResponse}">
/// The handler will wrap the response in a <see cref="Result{TResponse}" /> object">
/// </summary>
/// <typeparam name="TResponse">The type of the data entry that is returned.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
