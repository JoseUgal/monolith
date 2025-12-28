using Application.Messaging;

namespace Modules.Workspaces.Application.Workspaces.InviteMember;

/// <summary>
/// Represents the command for invite a new tenant member.
/// </summary>
/// <param name="WorkspaceId">The workspace identifier.</param>
/// <param name="RequestedUserId">The requested user identifier.</param>
/// <param name="UserId">The user identifier.</param>
/// <param name="Role">The role.</param>
public sealed record InviteWorkspaceMemberCommand(
    Guid WorkspaceId,
    Guid RequestedUserId,
    Guid UserId,
    string Role
) : ICommand<Guid>;
