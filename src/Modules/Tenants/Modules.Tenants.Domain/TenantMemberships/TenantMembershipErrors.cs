using Domain.Errors;

namespace Modules.Tenants.Domain.TenantMemberships;

/// <summary>
/// Contains the tenant membership errors.
/// </summary>
public static class TenantMembershipErrors
{
    /// <summary>
    /// Indicates that a tenant membership with the specified identifier was not found.
    /// </summary>
    /// <param name="membershipId">The unique identifier of the tenant membership that could not be found.</param>
    public static Error NotFound(TenantMembershipId membershipId) => Error.NotFound(
        "TenantMembership.NotFound",
        $"The tenant membership with the identifier '{membershipId.Value}' was not found."
    );
    
    /// <summary>
    /// Contains the role errors.
    /// </summary>
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

    /// <summary>
    /// Contains the invitation errors.
    /// </summary>
    public static class Invitation
    {
        /// <summary>
        /// Indicates that the tenant membership invitation is already accepted.
        /// </summary>
        public static Error AlreadyAccepted => Error.Conflict(
            "TenantMembership.Invitation.AlreadyAccepted",
            "The tenant membership invitation is already accepted."
        );
        
        /// <summary>
        /// Indicates that the tenant membership invitation is not valid.
        /// </summary>
        public static Error InvalidStatus => Error.Conflict(
            "TenantMembership.Invitation.InvalidStatus",
            "The tenant membership invitation is not valid."
        );

        /// <summary>
        /// Indicates that the tenant membership invitation is not for the specified user.
        /// </summary>
        public static Error NotForUser => Error.Forbidden(
            "TenantMembership.Invitation.NotForUser",
            "The tenant membership invitation is not for the specified user."
        );
    }
}
