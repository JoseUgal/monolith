using Application.ServiceLifetimes;
using Microsoft.EntityFrameworkCore;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Persistence.Repositories;

/// <summary>
/// Represents the tenant repository.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class TenantRepository(TenantsDbContext dbContext) : ITenantRepository, IScoped
{
    /// <inheritdoc />
    public void Add(Tenant tenant) => dbContext.Set<Tenant>().Add(tenant);

    /// <inheritdoc />
    public async Task<bool> IsSlugUniqueAsync(TenantSlug slug, CancellationToken cancellationToken = default)
    {
        return !await dbContext.Set<Tenant>().AnyAsync(tenant =>
            tenant.Slug == slug,
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<TenantMembership[]> GetMembersAsync(TenantId tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TenantMembership>().Where(member =>
            member.TenantId == tenantId
        ).ToArrayAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(TenantId tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Tenant>().AnyAsync(tenant => 
            tenant.Id == tenantId, 
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<Tenant?> GetWithMembershipsAsync(TenantId tenantId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Tenant>().Include(tenant => tenant.Memberships).FirstOrDefaultAsync(tenant =>
            tenant.Id == tenantId,
            cancellationToken
        );
    }
}
