using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Primitives;
using Domain.Results;

namespace Modules.Users.Domain.Users;

/// <summary>
/// Represents the user's email as an immutable value object.
/// </summary>
/// <remarks>
/// The email is normalized to lowercase invariant for consistent
/// comparisons and storage.
/// </remarks>
public sealed partial class UserEmail : ValueObject
{
    /// <summary>
    /// Gets the maximum allowed length.
    /// </summary>
    public const int MaxLength = 320;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEmail"/> class.
    /// </summary>
    /// <param name="value">The validated value.</param>
    /// <remarks>
    /// This constructor is intended for EF Core and mapping purposes only.
    /// It performs no validation. For domain-level creation and validation,
    /// use <see cref="Create(string)"/> instead.
    /// <summary>
/// Initializes a new <see cref="UserEmail"/> with the specified raw value.
/// </summary>
/// <param name="value">The email value to assign; expected to be already normalized. This constructor is intended for ORM mapping (e.g., EF Core) and performs no validation.</param>
    public UserEmail(string value) => Value = value;

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }
    
    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues() => [Value];
    
    /// <summary>
    /// Creates a validated email.
    /// </summary>
    /// <param name="email">The primitive string value.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing either a valid <see cref="UserEmail"/>
    /// or an error describing why validation failed.
    /// <summary>
    /// Validates the provided email string, normalizes it, and constructs a UserEmail value object on success.
    /// </summary>
    /// <param name="email">The email address to validate and convert into a UserEmail. May include surrounding whitespace or mixed case; the value will be normalized.</param>
    /// <returns>
    /// A successful Result containing a UserEmail with a normalized (trimmed, lowercase) value when validation passes; otherwise a failed Result with one of:
    /// - Error code "User.Email.IsRequired" when the input is null, empty, or whitespace.
    /// - Error code "User.Email.TooLong" when the normalized email length exceeds 320 characters.
    /// - Error code "User.Email.InvalidFormat" when the normalized email does not match the expected email pattern.
    /// </returns>
    public static Result<UserEmail> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<UserEmail>(
                Error.Failure(
                    "User.Email.IsRequired",
                    "The user email cannot be null or empty."
                )
            );
        }

        string normalized = Normalize(email);

        if (normalized.Length > MaxLength)
        {
            return Result.Failure<UserEmail>(
                Error.Failure(
                    "User.Email.TooLong",
                    "The user email cannot be longer than " + MaxLength
                )
            );
        }

        if (!EmailRegex().IsMatch(normalized))
        {
            return Result.Failure<UserEmail>(
                Error.Failure(
                    "User.Email.InvalidFormat",
                    "The user email must be a valid format."
                )
            );
        }

        return new UserEmail(normalized);
    }

    /// <summary>
/// Normalize an email by removing surrounding whitespace and converting to lowercase using the invariant culture.
/// </summary>
/// <param name="email">The input email to normalize.</param>
/// <returns>The normalized email string.</returns>
private static string Normalize(string email) => email.Trim().ToLowerInvariant();

    /// <summary>
    /// Provides a compiled regular expression for validating basic email address format.
    /// </summary>
    /// <returns>A <see cref="Regex"/> that matches the pattern <c>^[^\s@]+@[^\s@]+\.[^\s@]+$</c> using case-insensitive matching.</returns>
    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();
}