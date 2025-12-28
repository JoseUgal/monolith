using Domain.Primitives;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Domain.WorkspaceMemberships;

public sealed class WorkspaceMembership : Entity<WorkspaceMembershipId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceMembership"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="workspaceId">The workspace identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="role">The role.</param>
    /// <param name="status">The status.</param>
    private WorkspaceMembership(WorkspaceMembershipId id, WorkspaceId workspaceId, Guid userId, WorkspaceMembershipRole role, WorkspaceMembershipStatus status) : base(id)
    {
        WorkspaceId = workspaceId;
        UserId = userId;
        Role = role;
        Status = status;
    }
    
    /// <summary>
    /// Gets the workspace identifier.
    /// </summary>
    public WorkspaceId WorkspaceId { get; private set; }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets the role.
    /// </summary>
    public WorkspaceMembershipRole Role { get; private set; }

    /// <summary>
    /// Gets the status.
    /// </summary>
    public WorkspaceMembershipStatus Status { get; private set; }

    /// <summary>
    /// Creates the initial owner membership for a newly created workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The created <see cref="WorkspaceMembership"/>.</returns>
    public static WorkspaceMembership CreateOwner(WorkspaceId workspaceId, Guid userId)
    {
        return new WorkspaceMembership(
            new WorkspaceMembershipId(Guid.NewGuid()),
            workspaceId,
            userId,
            WorkspaceMembershipRole.Owner,
            WorkspaceMembershipStatus.Active
        );
    }

    /// <summary>
    /// Invites a user to join the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="role">The role.</param>
    /// <returns>The created <see cref="WorkspaceMembership"/>.</returns>
    public static WorkspaceMembership Invite(WorkspaceId workspaceId, Guid userId, WorkspaceMembershipRole role)
    {
        return new WorkspaceMembership(
            new WorkspaceMembershipId(Guid.NewGuid()),
            workspaceId,
            userId,
            role,
            WorkspaceMembershipStatus.Invited
        );
    }
}
