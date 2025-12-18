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
}
