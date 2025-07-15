using DA.Messaging.Abstractions;
using DA.Results;

namespace DA.Messaging.Commands;

/// <summary>
/// Handler for an <see cref="ICreateCommand{TCreated}"/>
/// </summary>
/// <typeparam name="TRequest">The type of the request to handle.</typeparam>
/// <typeparam name="TCreated">The type of the object to create.</typeparam>
public interface ICreateCommandHandler<TRequest, TCreated> : IRequestHandler<TRequest, Result<TCreated>>
    where TRequest : ICreateCommand<TCreated>;
