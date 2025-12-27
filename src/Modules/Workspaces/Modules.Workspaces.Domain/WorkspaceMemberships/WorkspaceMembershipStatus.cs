namespace Modules.Workspaces.Domain.WorkspaceMemberships;

/// <summary>
/// Enumerates the possible statuses of a workspace membership.
/// </summary>
public enum WorkspaceMembershipStatus
{
    /// <summary>
    /// The membership is invited.
    /// </summary>
    Invited = 0,
    
    /// <summary>
    /// The membership is active.
    /// </summary>
    Active = 1
}
