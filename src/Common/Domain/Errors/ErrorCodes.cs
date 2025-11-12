namespace Domain.Errors;

/// <summary>
/// Contains generic error codes that can be reused across the application.
/// </summary>
public static class ErrorCodes
{
    /// <summary>
    /// Error indicating that a value was null when it should not be.
    /// </summary>
    public const string NotNull = "Error.NotNull";

    /// <summary>
    /// Error indicating that a required condition was not met.
    /// </summary>
    public const string ConditionNotMet = "Error.ConditionNotMet";

    /// <summary>
    /// Error indicating a general validation issue.
    /// </summary>
    public const string ValidationGeneral = "Error.Validation.General";
}
