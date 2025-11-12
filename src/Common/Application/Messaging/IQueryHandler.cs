using Domain.Results;
using MediatR;

namespace Application.Messaging;

/// <summary>
/// Handles a query of type <typeparamref name="TQuery"/> that produces a typed <see cref="Result{TResponse}"/>.
/// Inherits from MediatR's <see cref="IRequestHandler{TRequest,TResponse}"/>.
/// </summary>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;
