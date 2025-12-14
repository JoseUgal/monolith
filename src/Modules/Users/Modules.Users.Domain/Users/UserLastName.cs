using Domain.Errors;
using Domain.Primitives;
using Domain.Results;

namespace Modules.Users.Domain.Users;

/// <summary>
/// Represents the user's last name as an immutable value object.
/// </summary>
/// <remarks>
/// Leading and trailing spaces are trimmed to maintain consistency.
/// </remarks>
public sealed class UserLastName : ValueObject
{
    /// <summary>
    /// Gets the maximum allowed length.
    /// </summary>
    public const int MaxLength = 150;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserLastName"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <remarks>
    /// This constructor is intended for EF Core and mapping purposes only.
    /// It performs no validation. For domain-level creation and validation,
    /// use <see cref="Create(string)"/> instead.
    /// <summary>
/// Initializes a new instance of <see cref="UserLastName"/> with the specified value for persistence and mapping.
/// </summary>
/// <param name="value">The last name value to assign. This constructor does not perform validation and is intended for ORM/serialization use.</param>
    public UserLastName(string value) => Value = value;
    
    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }

    /// <summary>
/// Provides the atomic value used for equality comparisons of this value object.
/// </summary>
/// <returns>An enumerable containing a single element: the last name string.</returns>
    protected override IEnumerable<object> GetAtomicValues() => [Value];

    /// <summary>
    /// Creates a validated last name.
    /// </summary>
    /// <param name="lastName">The primitive string value.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing either a valid <see cref="UserLastName"/>
    /// or an error describing why validation failed.
    /// <summary>
    /// Creates a validated <see cref="UserLastName"/> value object from the provided string.
    /// </summary>
    /// <param name="lastName">Candidate last name; leading and trailing whitespace are trimmed before validation.</param>
    /// <returns>
    /// A <see cref="Result{UserLastName}"/> containing the created <see cref="UserLastName"/> on success.
    /// On failure, contains an <c>Error</c> with code <c>User.LastName.IsRequired</c> when the input is null, empty, or whitespace,
    /// or <c>User.LastName.TooLong</c> when the trimmed input exceeds <see cref="MaxLength"/>.
    /// </returns>
    public static Result<UserLastName> Create(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<UserLastName>(
                Error.Failure(
                    "User.LastName.IsRequired",
                    "The user last name cannot be null or empty."
                )
            );
        }
        
        lastName = lastName.Trim();

        if (lastName.Length > MaxLength)
        {
            return Result.Failure<UserLastName>(
                Error.Failure(
                    "User.LastName.TooLong",
                    "The user last name cannot be longer than " + MaxLength
                )
            );
        }

        return new UserLastName(lastName);
    }
}