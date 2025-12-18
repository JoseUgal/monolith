namespace Modules.Tenants.Domain.TenantMemberships;

/// <summary>
/// The status of a tenant membership.
/// </summary>
public enum TenantMembershipStatus
{
    /// <summary>
    /// The membership is invited.
    /// </summary>
    Invited = 0,
    
    /// <summary>
    /// The membership is active.
    /// </summary>
    Active = 1
}
