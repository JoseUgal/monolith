namespace Modules.Tenants.Domain.TenantMemberships;

/// <summary>
/// The role of a tenant membership.
/// </summary>
public enum TenantRole
{
    /// <summary>
    /// The owner of the tenant.
    /// </summary>
    Owner,
    
    /// <summary>
    /// The admin of the tenant.
    /// </summary>
    Admin,
    
    /// <summary>
    /// The billing of the tenant.
    /// </summary>
    Billing,
    
    /// <summary>
    /// The member of the tenant.
    /// </summary>
    Member
}
