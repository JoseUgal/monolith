using System.Text.RegularExpressions;
using Domain.Errors;
using Domain.Primitives;
using Domain.Results;

namespace Modules.Users.Domain.Users;

/// <summary>
/// Represents a raw password value before hashing.
/// </summary>
/// <remarks>
/// This value object only validates password policy rules. 
/// It does not hash or transform the value in any way, as hashing 
/// must be handled by the infrastructure layer.
/// </remarks>
public sealed class UserPassword : ValueObject
{
    /// <summary>
    /// Gets the minimum allowed length.
    /// </summary>
    public const int MinLength = 8;

    /// <summary>
    /// Gets the maximum allowed length.
    /// </summary>
    public const int MaxLength = 128;
    
    public const int HashMaxLength = 200;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPassword"/> class.
    /// </summary>
    /// <param name="value">The validated password.</param>
    private UserPassword(string value) => Value = value;

    /// <summary>
    /// Gets the raw password value.
    /// </summary>
    /// <remarks>
    /// This value is intended to be used immediately by a password hasher.
    /// It should never be stored as plain text.
    /// </remarks>
    public string Value { get; }

    /// <inheritdoc />
    protected override IEnumerable<object> GetAtomicValues() => [Value];

    /// <summary>
    /// Creates a validated password using predefined password policy rules.
    /// </summary>
    /// <param name="password">The raw password value.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing either a valid <see cref="UserPassword"/>
    /// or an error describing why validation failed.
    /// </returns>
    public static Result<UserPassword> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result.Failure<UserPassword>(
                Error.Failure(
                    "User.Password.IsRequired",
                    "The password cannot be null or empty."
                )
            );
        }

        if (password.Length < MinLength)
        {
            return Result.Failure<UserPassword>(
                Error.Failure(
                    "User.Password.TooShort",
                    $"The password must be at least {MinLength} characters long."
                )
            );
        }

        if (password.Length > MaxLength)
        {
            return Result.Failure<UserPassword>(
                Error.Failure(
                    "User.Password.TooLong",
                    $"The password cannot be longer than {MaxLength} characters."
                )
            );
        }

        if (!HasRequiredComplexity(password))
        {
            return Result.Failure<UserPassword>(
                Error.Failure(
                    "User.Password.Weak",
                    "The password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character."
                )
            );
        }

        return new UserPassword(password);
    }

    private static bool HasRequiredComplexity(string password)
    {
        bool hasUpper = Regex.IsMatch(password, "[A-Z]");
        bool hasLower = Regex.IsMatch(password, "[a-z]");
        bool hasDigit = Regex.IsMatch(password, "[0-9]");
        bool hasSpecial = Regex.IsMatch(password, "[!@#$%^&*(),.?:{}|<>_+=\\-\\[\\]\\/]");
        
        return hasUpper && hasLower && hasDigit && hasSpecial;
    }
}

