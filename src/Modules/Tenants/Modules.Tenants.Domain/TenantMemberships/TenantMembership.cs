using Domain.Primitives;
using Domain.Results;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Domain.TenantMemberships;

public sealed class TenantMembership : Entity<TenantMembershipId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TenantMembership"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="role">The role.</param>
    /// <param name="status">The status.</param>
    private TenantMembership(TenantMembershipId id, TenantId tenantId, Guid userId, TenantRole role, TenantMembershipStatus status) : base(id)
    {
        TenantId = tenantId;
        UserId = userId;
        Role = role;
        Status = status;
    }
    
    /// <summary>
    /// Gets the tenant identifier.
    /// </summary>
    public TenantId TenantId { get; init; }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets the role.
    /// </summary>
    public TenantRole Role { get; private set; }

    /// <summary>
    /// Gets the status.
    /// </summary>
    public TenantMembershipStatus Status { get; private set; }
    
    /// <summary>
    /// Creates the initial Owner membership for a newly created tenant.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The created <see cref="TenantMembership"/>.</returns>
    public static TenantMembership CreateOwner(TenantId tenantId, Guid userId)
    {
        return new TenantMembership(
            new TenantMembershipId(Guid.NewGuid()),
            tenantId,
            userId,
            TenantRole.Owner,
            TenantMembershipStatus.Active
        );
    }
    
    /// <summary>
    /// Invites a user to join the tenant.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="role">The role.</param>
    /// <returns>The created <see cref="TenantMembership"/>.</returns>
    public static TenantMembership Invite(TenantId tenantId, Guid userId, TenantRole role)
    {
        return new TenantMembership(
            new TenantMembershipId(Guid.NewGuid()),
            tenantId,
            userId,
            role,
            TenantMembershipStatus.Invited
        );
    }

    /// <summary>
    /// Accepts the invitation for this tenant membership.
    /// </summary>
    /// <returns>The result of the operation.</returns>
    public Result Accept()
    {
        if (Status == TenantMembershipStatus.Active)
        {
            return Result.Failure(
                TenantMembershipErrors.Invitation.AlreadyAccepted    
            );
        }

        if (Status != TenantMembershipStatus.Invited)
        {
            return Result.Failure(
                TenantMembershipErrors.Invitation.InvalidStatus    
            );
        }
        
        Status = TenantMembershipStatus.Active;
        
        return Result.Success();
    }
}
