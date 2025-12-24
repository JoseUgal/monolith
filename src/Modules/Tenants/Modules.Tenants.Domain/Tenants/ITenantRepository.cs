using Modules.Tenants.Domain.TenantMemberships;

namespace Modules.Tenants.Domain.Tenants;

/// <summary>
/// Represents the tenant repository interface.
/// </summary>
public interface ITenantRepository
{
    /// <summary>
    /// Adds the specified tenant to the repository.
    /// </summary>
    /// <param name="tenant">The tenant.</param>
    void Add(Tenant tenant);
    
    /// <summary>
    /// Checks if the specified slug is unique.
    /// </summary>
    /// <param name="slug">The slug.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The success result if the slug is unique, otherwise a failure result.</returns>
    Task<bool> IsSlugUniqueAsync(TenantSlug slug, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the members of the specified tenant.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of members.</returns>
    Task<TenantMembership[]> GetMembersAsync(TenantId tenantId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if the specified tenant exists.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The success result if the tenant exists, otherwise a failure result.</returns>
    Task<bool> ExistsAsync(TenantId tenantId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the tenant with the specified identifier.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The tenant with the specified identifier.</returns>
    /// <remarks>
    /// This method returns the tenant with its memberships.
    /// </remarks>
    Task<Tenant?> GetWithMembershipsAsync(TenantId tenantId, CancellationToken cancellationToken = default);
}
