using Domain.Errors;

namespace Modules.Tenants.Domain.TenantMemberships;

/// <summary>
/// Contains the tenants errors.
/// </summary>
public static class TenantMembershipErrors
{
    public static class Role
    {
        /// <summary>
        /// Indicates that the tenant membership role is invalid.
        /// </summary>
        public static Error IsInvalid => Error.Failure(
            "TenantMembership.Role.IsInvalid",
            "The tenant membership role is not defined."
        );
    }
}
