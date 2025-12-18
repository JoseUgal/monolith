using Domain.Errors;

namespace Modules.Tenants.Domain.Tenants;

/// <summary>
/// Contains the tenants errors.
/// </summary>
public static class TenantErrors
{
    /// <summary>
    /// Indicates that the user is already a member of this tenant.
    /// </summary>
    public static Error MemberAlreadyExist => Error.Conflict(
        "Tenant.MemberAlreadyExist",
        "The user is already a member of this tenant."
    );
    
    /// <summary>
    /// Indicates that the owner can only be added at tenant creation.
    /// </summary>
    public static Error OwnerAlreadyExist => Error.Conflict(
        "Tenant.OwnerAlreadyExist",
        "The owner can only be added at tenant creation."
    );
    
    /// <summary>
    /// Contains validation errors related to the tenant's name.
    /// </summary>
    public static class Name
    {
        /// <summary>
        /// Indicates that the tenant's name is required.
        /// </summary>
        public static Error IsRequired => Error.Failure(
            "Tenant.Name.IsRequired",
            "The tenant name cannot be null or empty."
        );
    }

    /// <summary>
    /// Contains validation errors related to the tenant's name.
    /// </summary>
    public static class Slug
    {
        /// <summary>
        /// Indicates that the tenant's slug is required.
        /// </summary>
        public static Error IsRequired => Error.Failure(
            "Tenant.Slug.IsRequired",
            "The tenant slug cannot be null or empty."
        );
        
        /// <summary>
        /// Indicates that the specified slug is already in use by another tenant.
        /// </summary>
        public static Error IsNotUnique => Error.Conflict(
            "Tenant.Slug.IsNotUnique", 
            "The specified slug is already in use."
        );
    }
}
