namespace Domain.Errors;

/// <summary>
/// Represents the validation result containing an array of errors.
/// </summary>
public sealed record ValidationError : Error
{
    /// <summary>
    /// Initializes a new instance of <see cref="ValidationError"/> with one or more errors.
    /// </summary>
    /// <param name="errors">The errors to include in the validation error.</param>
    public ValidationError(params Error[] errors) : base(ErrorCodes.ValidationGeneral, "One or more validation errors occurred", ErrorType.Validation)
    {
        Errors = errors;
    }

    /// <summary>
    /// Gets the collection of validation errors.
    /// </summary>
    public Error[] Errors { get; }

    /// <summary>
    /// Returns <c>true</c> if there is at least one error in <see cref="Errors"/>.
    /// </summary>
    public bool HasErrors => Errors.Length > 0;

    /// <summary>
    /// Creates a new ValidationError with additional errors appended.
    /// </summary>
    /// <param name="additionalErrors">Errors to append.</param>
    /// <returns>A new ValidationError containing all errors.</returns>
    public ValidationError WithErrors(params Error[] additionalErrors)
    {
        if (additionalErrors.Length == 0)
        {
            return this;
        }

        var combined = Errors.Concat(additionalErrors).ToArray();

        return new ValidationError(combined);
    }

    /// <summary>
    /// Returns a readable string representing all validation errors.
    /// </summary>
    public override string ToString()
    {
        if (Errors.Length == 0)
        {
            return base.ToString();
        }

        return $"{Type}: {string.Join(";", Errors.Select(e => $"{e.Code} -  {e.Description}"))}";
    }

}
