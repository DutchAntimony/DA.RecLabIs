using DA.Messaging.Requests.Abstractions;
using DA.Results;

namespace DA.Messaging.Requests.Commands;
/// <summary>
/// Request to modify the state of an exisiting object.
/// There is no return type, the caller must decide which action to take after the command is executed.
/// </summary>
/// <remarks>For commands that create something, there is the ICreateCommand which allows a return object.</remarks>
public interface ICommand : IRequest<Result>;
