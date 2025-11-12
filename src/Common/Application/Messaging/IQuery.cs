using Domain.Results;
using MediatR;

namespace Application.Messaging;

/// <summary>
/// Represents a query that produces a typed <see cref="Result{TResponse}"/> when handled.
/// Inherits from MediatR's <see cref="IRequest{TResponse}"/>.
/// </summary>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
