namespace Domain.Errors;

/// <summary>
/// Represents an error.
/// Supports equality comparison based on <see cref="Code"/> and <see cref="Type"/>.
/// </summary>
public record Error(string Code, string Description, ErrorType Type)
{
    /// <summary>
    /// The empty error instance.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    /// <summary>
    /// The null value error instance.
    /// </summary>
    public static readonly Error NullValue = new(ErrorCodes.NotNull, "The specified result value is null.", ErrorType.Failure);

    /// <summary>
    /// The condition not met error instance.
    /// </summary>
    public static readonly Error ConditionNotMet = new(ErrorCodes.ConditionNotMet, "The specified condition was not met.", ErrorType.Failure);

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with failure type.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Failure(string code, string description) => new(code, description, ErrorType.Failure);

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with not found type.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    public static Error NotFound(string code, string description) => new(code, description, ErrorType.NotFound);

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with problem type.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Problem(string code, string description) => new(code, description, ErrorType.Problem);

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with conflict type.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Conflict(string code, string description) => new(code, description, ErrorType.Conflict);

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with forbidden type.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    public static Error Forbidden(string code, string description) => new(code, description, ErrorType.Forbidden);

    /// <summary>
    /// Returns a copy of the current error with a new code.
    /// </summary>
    /// <param name="code">The new error code.</param>
    public Error WithCode(string code) => this with {  Code = code };

    /// <summary>
    /// Returns a copy of the current error with a new description.
    /// </summary>
    /// <param name="description">The new error description.</param>
    public Error WithDescription(string description) => this with {  Description = description };

    /// <summary>
    /// Determines whether the specified <see cref="Error"/> is equal to the current one.
    /// Equality is based on <see cref="Code"/> and <see cref="Type"/> only.
    /// </summary>
    /// <param name="other">The other error to compare.</param>
    /// <returns><c>true</c> if the errors have the same <see cref="Code"/> and <see cref="Type"/>; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Type == other.Type;
    }

    /// <summary>
    /// Returns a hash code for the error based on <see cref="Code"/> and <see cref="Type"/>.
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(Code, Type);

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    public override string ToString() => $"{Code} ({Type}): {Description}";
}
