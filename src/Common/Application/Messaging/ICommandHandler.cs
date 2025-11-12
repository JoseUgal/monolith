using Domain.Results;
using MediatR;

namespace Application.Messaging;

/// <summary>
/// Handles a command of type <typeparamref name="TCommand"/> that produces a <see cref="Result"/>.
/// Inherits from MediatR's <see cref="IRequestHandler{TRequest,TResponse}"/>.
/// </summary>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;

/// <summary>
/// Handles a command of type <typeparamref name="TCommand"/> that produces a typed <see cref="Result{TResponse}"/>.
/// Inherits from MediatR's <see cref="IRequestHandler{TRequest,TResponse}"/>.
/// </summary>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>;
