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
}
