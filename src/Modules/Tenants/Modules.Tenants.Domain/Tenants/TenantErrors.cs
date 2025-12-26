using Domain.Errors;

namespace Modules.Tenants.Domain.Tenants;

/// <summary>
/// Contains the tenants errors.
/// </summary>
public static class TenantErrors
{
    /// <summary>
    /// Indicates that a tenant with the specified identifier was not found.
    /// </summary>
    /// <param name="tenantId">The unique identifier of the tenant that could not be found.</param>
    public static Error NotFound(TenantId tenantId) => Error.NotFound(
        "Tenant.NotFound",
        $"The tenant with the identifier '{tenantId.Value}' was not found."
    );
    
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
        
        /// <summary>
        /// Indicates that the tenant's name is shorter than the minimum required length.
        /// </summary>
        /// <param name="minLength">
        /// The minimum number of characters required for the tenant name.
        /// </param>
        public static Error TooShort(int minLength) => Error.Failure(
            "Tenant.Name.TooShort",
            $"The name must be at least {minLength} characters long."
        );

        /// <summary>
        /// Indicates that the tenant's name exceeds the maximum allowed length.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the tenant name.</param>
        public static Error TooLong(int maxLength) => Error.Failure(
            "Tenant.Name.TooLong",
            $"The name cannot be longer than {maxLength} characters."
        );
    }

    /// <summary>
    /// Contains validation errors related to the tenant's slug.
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
        /// Indicates that the tenant's slug is invalid.
        /// </summary>
        public static Error IsInvalid => Error.Failure(
            "Tenant.Slug.IsInvalid",
            "The tenant slug does not match the required format."
        );
        
        /// <summary>
        /// Indicates that the specified slug is already in use by another tenant.
        /// </summary>
        public static Error IsNotUnique => Error.Conflict(
            "Tenant.Slug.IsNotUnique", 
            "The specified slug is already in use."
        );
        
        /// <summary>
        /// Indicates that the tenant's slug is shorter than the minimum required length.
        /// </summary>
        /// <param name="minLength">
        /// The minimum number of characters required for the tenant slug.
        /// </param>
        public static Error TooShort(int minLength) => Error.Failure(
            "Tenant.Slug.TooShort",
            $"The slug must be at least {minLength} characters long."
        );

        /// <summary>
        /// Indicates that the tenant's slug exceeds the maximum allowed length.
        /// </summary>
        /// <param name="maxLength">The maximum allowed length for the tenant slug.</param>
        public static Error TooLong(int maxLength) => Error.Failure(
            "Tenant.Slug.TooLong",
            $"The slug cannot be longer than {maxLength} characters."
        );
    }
}
