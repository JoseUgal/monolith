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
    public static Tenant Create(TenantName name, TenantSlug slug, Guid ownerId)
    {
        var id = new TenantId(Guid.NewGuid());
        
        var tenant = new Tenant(id, name, slug);

        tenant.AddOwner(ownerId);
        
        return tenant;
    }
    
    /// <summary>
    /// Invites a user to this tenant with a role.
    /// </summary>
    public Result<TenantMembership> InviteMember(Guid userId, TenantRole role)
    {
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
    /// Adds the first Owner membership for this tenant.
    /// </summary>
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
