using Application.Messaging;

namespace Modules.Workspaces.Application.Workspaces.AcceptInvitation;

/// <summary>
/// Represents the command for accepting a workspace invitation.
/// </summary>
/// <param name="WorkspaceId">The workspace identifier.</param>
/// <param name="MembershipId">The workspace membership identifier.</param>
/// <param name="UserId">The current user identifier.</param>
public sealed record AcceptWorkspaceInvitationCommand(
    Guid WorkspaceId,
    Guid MembershipId,
    Guid UserId
) : ICommand;
