namespace Modules.Workspaces.Endpoints.Workspaces.InviteMember;

/// <summary>
/// Represents the request for inviting a new workspace member.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="Role">The role.</param>
public sealed record InviteWorkspaceMemberRequest(
    Guid UserId, 
    string Role
);
