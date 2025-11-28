using Domain.Results;
using MediatR;

namespace Application.Messaging;

/// <summary>
/// Represents a command that does not return a specific result type,
/// producing a <see cref="Result"/> when handled.
/// Inherits from MediatR's <see cref="IRequest{TResponse}"/> and <see cref="IBaseCommand"/>.
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand;

/// <summary>
/// Represents a command that returns a specific result type <typeparamref name="TResponse"/> 
/// when handled, producing a <see cref="Result{TResponse}"/>.
/// Inherits from MediatR's <see cref="IRequest{TResponse}"/> and <see cref="IBaseCommand"/>.
/// </summary>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

/// <summary>
/// Marker interface for all commands. Used to identify command types in the system.
/// </summary>
public interface IBaseCommand;
