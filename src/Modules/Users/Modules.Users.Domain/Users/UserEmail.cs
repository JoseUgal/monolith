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
    /// </remarks>
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

    private static string Normalize(string email) => email.Trim().ToLowerInvariant();

    [GeneratedRegex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();
}
