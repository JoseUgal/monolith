namespace Domain.Errors;

/// <summary>
/// Represents different types of errors that can occur within the application.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// A general failure occurred.
    /// </summary>
    Failure = 0,

    /// <summary>
    /// Validation errors were encountered (e.g., invalid data).
    /// </summary>
    Validation = 1,

    /// <summary>
    /// A problem occurred that requires further investigation.
    /// </summary>
    Problem = 2,

    /// <summary>
    /// The requested resource was not found.
    /// </summary>
    NotFound = 3,

    /// <summary>
    /// A conflict occurred (e.g., attempting to create a duplicate resource).
    /// </summary>
    Conflict = 4
}
