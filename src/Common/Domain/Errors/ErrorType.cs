namespace Domain.Errors;

/// <summary>
/// Represents different types of errors that can occur within the application.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// A general failure occurred that does not fit into a more specific category.
    /// </summary>
    /// <remarks>
    /// Typically used for unexpected or generic business failures.
    /// </remarks>
    Failure = 0,

    /// <summary>
    /// One or more validation rules were violated.
    /// </summary>
    /// <remarks>
    /// Used when input data is syntactically correct but semantically invalid
    /// (e.g. missing required fields, invalid formats, violated invariants).
    /// </remarks>
    Validation = 1,

    /// <summary>
    /// An unexpected problem occurred that requires further investigation.
    /// </summary>
    /// <remarks>
    /// Used for technical or infrastructural issues that are not caused
    /// by user input or business rule violations.
    /// </remarks>
    Problem = 2,

    /// <summary>
    /// The requested resource could not be found.
    /// </summary>
    /// <remarks>
    /// Used when the target entity does not exist or is not accessible
    /// within the given context.
    /// </remarks>
    NotFound = 3,

    /// <summary>
    /// The requested operation could not be completed due to a conflict
    /// with the current state of the resource.
    /// </summary>
    /// <remarks>
    /// Used for state conflicts such as duplicate creation, invalid state transitions,
    /// or attempts to apply an operation that has already been applied.
    /// </remarks>
    Conflict = 4,

    /// <summary>
    /// The user is authenticated, but does not have permission to perform
    /// the requested operation on the specified resource.
    /// </summary>
    /// <remarks>
    /// Used when the identity is known, but authorization rules deny access.
    /// </remarks>
    Forbidden = 5
}
