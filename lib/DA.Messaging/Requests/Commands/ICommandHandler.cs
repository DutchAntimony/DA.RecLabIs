﻿using DA.Messaging.Requests.Abstractions;
using DA.Results;

namespace DA.Messaging.Requests.Commands;

/// <summary>
/// Handler for an <see cref="ICommand"/>
/// </summary>
/// <typeparam name="TRequest">The type of the request to handle.</typeparam>
public interface ICommandHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : ICommand;
