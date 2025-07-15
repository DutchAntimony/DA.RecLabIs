using DA.Messaging.Requests.Abstractions;
using DA.Results;

namespace DA.Messaging.Requests.Commands;

/// <summary>
/// Request to create an object.
/// The handler will wrap the response in a <see cref="Result{TCreated}" /> object"
/// </summary>
/// <remarks>This method allows the caller to get information about what is created, 
/// such that the appropriate response can be made by the caller.</remarks>
/// <typeparam name="TCreated">The type of the object to create.</typeparam>
public interface ICreateCommand<TCreated> : IRequest<Result<TCreated>>;
