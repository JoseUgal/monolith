namespace Modules.Workspaces.Domain.WorkspaceMemberships;

/// <summary>
/// Enumerates the possible roles of a workspace membership.
/// </summary>
public enum WorkspaceMembershipRole
{
    /// <summary>
    /// The owner of the workspace.
    /// </summary>
    Owner = 0,
    
    /// <summary>
    /// The admin of the workspace.
    /// </summary>
    Admin = 1,
    
    /// <summary>
    /// The member of the workspace.
    /// </summary>
    Member = 2
}
