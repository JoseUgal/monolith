using System.Text.RegularExpressions;
using Domain.Primitives;
using Domain.Results;

namespace Modules.Tenants.Domain.Tenants;

/// <summary>
/// Represents the tenant's slug as an immutable value object.
/// </summary>
public sealed class TenantSlug : ValueObject
{
    /// <summary>
    /// Gets the minimum allowed length.
    /// </summary>
    public const int MinLength = 2;
    
    /// <summary>
    /// Gets the maximum allowed length.
    /// </summary>
    public const int MaxLength = 50;
    
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
        
        if (normalized.Length < MinLength)
        {
            return Result.Failure<TenantSlug>(
                TenantErrors.Slug.TooShort(MinLength)
            );
        }

        if (normalized.Length > MaxLength)
        {
            return Result.Failure<TenantSlug>(
                TenantErrors.Slug.TooLong(MaxLength)
            );
        }
        
        if (!Pattern.IsMatch(normalized))
        {
            return Result.Failure<TenantSlug>(
                TenantErrors.Slug.IsInvalid
            );
        }
        
        return new TenantSlug(normalized);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantSlug"/> class.
    /// </summary>
    /// <param name="value">The validated slug value.</param>
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
    
    private static readonly Regex Pattern = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);
}
