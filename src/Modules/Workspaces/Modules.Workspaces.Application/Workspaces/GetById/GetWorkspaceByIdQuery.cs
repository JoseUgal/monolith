using Application.Messaging;

namespace Modules.Workspaces.Application.Workspaces.GetById;

/// <summary>
/// Represents the query for getting a workspace by its identifier.
/// </summary>
/// <param name="WorkspaceId">The workspace identifier.</param>
public sealed record GetWorkspaceByIdQuery(Guid WorkspaceId) : IQuery<WorkspaceResponse>;
