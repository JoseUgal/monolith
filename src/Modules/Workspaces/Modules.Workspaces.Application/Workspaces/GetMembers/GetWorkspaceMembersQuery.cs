using Application.Messaging;

namespace Modules.Workspaces.Application.Workspaces.GetMembers;

/// <summary>
/// Represents the query for getting a workspace members.
/// </summary>
/// <param name="WorkspaceId">The workspace identifier.</param>
public sealed record GetWorkspaceMembersQuery(Guid WorkspaceId) : IQuery<WorkspaceMemberResponse[]>;
