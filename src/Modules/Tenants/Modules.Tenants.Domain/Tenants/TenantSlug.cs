using Domain.Primitives;
using Domain.Results;

namespace Modules.Tenants.Domain.Tenants;

/// <summary>
/// Represents the tenant's slug as an immutable value object.
/// </summary>
public sealed class TenantSlug : ValueObject
{
    /// <summary>
    /// Creates a validated slug.
    /// </summary>
    /// <param name="slug">The primitive slug.</param>
    /// <returns>
    /// A <see cref="Result"/> containing either a valid <see cref="TenantSlug"/>
    /// or an error describing why validation failed.
    /// </returns>
    public static Result<TenantSlug> Create(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            return Result.Failure<TenantSlug>(
                TenantErrors.Slug.IsRequired
            );
        }
        
        string normalized = Normalize(slug);
        
        return new TenantSlug(normalized);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantSlug"/> class.
    /// </summary>
    /// <param name="value">The validated first name.</param>
    /// <remarks>
    /// This constructor is intended for EF Core and mapping purposes only.
    /// It performs no validation. For domain-level creation and validation,
    /// use <see cref="Create(string)"/> instead.
    /// </remarks>
    public TenantSlug(string value) => Value = value;
    
    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues() => [Value];
    
    private static string Normalize(string slug) => slug.Trim().ToLowerInvariant();
}
