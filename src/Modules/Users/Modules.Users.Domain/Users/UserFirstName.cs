using Domain.Errors;
using Domain.Primitives;
using Domain.Results;

namespace Modules.Users.Domain.Users;

/// <summary>
/// Represents the user's first name as an immutable value object.
/// </summary>
/// <remarks>
/// Leading and trailing spaces are trimmed to maintain consistency.
/// </remarks>
public sealed class UserFirstName : ValueObject
{
    /// <summary>
    /// Gets the maximum allowed length.
    /// </summary>
    public const int MaxLength = 150;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserFirstName"/> class.
    /// </summary>
    /// <param name="value">The validated first name.</param>
    /// <remarks>
    /// This constructor is intended for EF Core and mapping purposes only.
    /// It performs no validation. For domain-level creation and validation,
    /// use <see cref="Create(string)"/> instead.
    /// <summary>
/// Initializes a new instance of <see cref="UserFirstName"/> with the specified raw value for persistence/mapping scenarios.
/// </summary>
/// <param name="value">The stored first name value (may be unvalidated and is used for ORM mapping such as EF Core).</param>
/// <remarks>
/// This constructor does not perform domain validation and exists to support persistence frameworks. Use <see cref="Create(string)"/> to construct a validated instance.
/// </remarks>
    public UserFirstName(string value) => Value = value;
    
    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }

    /// <summary>
/// Provides the sequence of atomic values used to determine equality for this value object.
/// </summary>
/// <returns>An enumerable containing the underlying first name value.</returns>
    protected override IEnumerable<object> GetAtomicValues() => [Value];

    /// <summary>
    /// Creates a validated first name.
    /// </summary>
    /// <param name="firstName">The primitive string value.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing either a valid <see cref="UserFirstName"/>
    /// or an error describing why validation failed.
    /// <summary>
    /// Creates a validated UserFirstName from the given string.
    /// </summary>
    /// <param name="firstName">Candidate first name; leading and trailing whitespace are removed before validation.</param>
    /// <returns>A successful Result containing the created UserFirstName when validation passes; otherwise a failed Result with an Error describing the validation failure.</returns>
    public static Result<UserFirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<UserFirstName>(
                Error.Failure(
                    "User.FirstName.IsRequired",
                    "The user first name cannot be null or empty."
                )
            );
        }
        
        firstName = firstName.Trim();

        if (firstName.Length > MaxLength)
        {
            return Result.Failure<UserFirstName>(
                Error.Failure(
                    "User.FirstName.TooLong",
                    $"The user first name cannot be longer than {MaxLength} characters."
                )
            );
        }

        return new UserFirstName(firstName);
    }
}