using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Domain.Tests.Common.Mothers;

public static class TenantMother
{
    public static Tenant Create(
        string? name = null,
        string? slug = null,
        Guid? ownerId = null
    )
    {
        ownerId ??= Guid.NewGuid();
        
        TenantName tenantName = TenantName.Create(name ?? "demo").Value;
        TenantSlug tenantSlug = TenantSlug.Create(slug ?? "demo").Value;
        
        return Tenant.Create(tenantName, tenantSlug, ownerId.Value).Value;
    }
    
    public static Tenant CreateWithInvitedMemberships(
        params (Guid MemberId, TenantRole role)[] memberships
    )
    {
        Tenant tenant = Create();

        foreach ((Guid MemberId, TenantRole Role) membership in memberships)
        {
            tenant.InviteMember(membership.MemberId, membership.Role);
        }

        return tenant;
    }
    
    public static Tenant CreateWithActivatedMemberships(
        params (Guid MemberId, TenantRole role)[] memberships
    )
    {
        Tenant tenant = Create();

        foreach ((Guid MemberId, TenantRole Role) membership in memberships)
        {
            TenantMembership member = tenant.InviteMember(membership.MemberId, membership.Role).Value;

            tenant.AcceptInvitation(member.Id, member.UserId);
        }

        return tenant;
    }

}
