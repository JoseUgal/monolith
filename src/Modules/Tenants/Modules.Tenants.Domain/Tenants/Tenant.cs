using Domain.Primitives;
using Domain.Results;
using Modules.Tenants.Domain.TenantMemberships;

namespace Modules.Tenants.Domain.Tenants;

public sealed class Tenant : Entity<TenantId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Tenant"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="slug">The slug.</param>
    private Tenant(TenantId id, TenantName name, TenantSlug slug) : base(id)
    {
        Name = name;
        Slug = slug;
    }
    
    private readonly List<TenantMembership> _memberships = [];

    /// <summary>
    /// Gets the tenant memberships.
    /// </summary>
    public IReadOnlyCollection<TenantMembership> Memberships => _memberships.AsReadOnly();
    
    /// <summary>
    /// Gets the name.
    /// </summary>
    public TenantName Name { get; private set; }

    /// <summary>
    /// Gets the slug.
    /// </summary>
    public TenantSlug Slug { get; private set; }
    
    /// <summary>
    /// Creates a new <see cref="Tenant"/> entity.
    /// </summary>
    /// <remarks>
    /// The method generates a new <see cref="TenantId"/> for the entity.
    /// All value objects should be validated in this method.
    /// </remarks>
    public static Result<Tenant> Create(TenantName name, TenantSlug slug, Guid ownerId)
    {
        var id = new TenantId(Guid.NewGuid());
        
        var tenant = new Tenant(id, name, slug);

        Result result = tenant.AddOwner(ownerId);

        if (result.IsFailure)
        {
            return Result.Failure<Tenant>(result.Error);
        }
        
        return tenant;
    }
    
    /// <summary>
    /// Invites a user to this tenant with a role.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <param name="role">The role.</param>
    /// <returns>The result of the operation.</returns>
    public Result<TenantMembership> InviteMember(Guid userId, TenantRole role)
    {
        if (role == TenantRole.Owner)
        {
            return Result.Failure<TenantMembership>(
                TenantErrors.OwnerAlreadyExist
            );
        }
        
        if (_memberships.Any(m => m.UserId == userId))
        {
            return Result.Failure<TenantMembership>(
                TenantErrors.MemberAlreadyExist
            );
        }

        var membership = TenantMembership.Invite(Id, userId, role);
        
        _memberships.Add(membership);

        return membership;
    }

    /// <summary>
    /// Accepts the invitation for the specified tenant membership.
    /// </summary>
    /// <param name="membershipId">The identifier of the tenant membership.</param>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>The result of the operation.</returns>
    public Result AcceptInvitation(TenantMembershipId membershipId, Guid userId)
    {
        TenantMembership? membership = _memberships.SingleOrDefault(m => m.Id == membershipId);

        if (membership == null)
        {
            return Result.Failure(
                TenantMembershipErrors.NotFound(membershipId)    
            );
        }

        if (membership.UserId != userId)
        {
            return Result.Failure(
                TenantMembershipErrors.Invitation.NotForUser    
            );
        }

        return membership.Accept();
    }
    
    /// <summary>
    /// Adds the first Owner membership for this tenant.
    /// </summary>
    /// <param name="userId">The identifier of the user.</param>
    /// <returns>The result of the operation.</returns>
    private Result AddOwner(Guid userId)
    {
        if (_memberships.Count != 0)
        {
            return Result.Failure(
                TenantErrors.OwnerAlreadyExist    
            );
        }

        _memberships.Add(TenantMembership.CreateOwner(Id, userId));
        
        return Result.Success();
    }
}
