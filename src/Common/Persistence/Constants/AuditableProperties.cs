namespace Persistence.Constants;

/// <summary>
/// Contains the name of the shadow properties used for auditing entities.
/// </summary>
/// <remarks>
/// These properties are used in EF Core configurations and interceptors.
/// </remarks>
public static class AuditableProperties
{
    /// <summary>
    /// The name of the property storing the UTC creation timestamp.
    /// </summary>
    public const string CreatedOnUtc = nameof(CreatedOnUtc);
    
    /// <summary>
    /// The name of the property storing the UTC last modification timestamp.
    /// </summary>
    public const string ModifiedOnUtc = nameof(ModifiedOnUtc);
}

