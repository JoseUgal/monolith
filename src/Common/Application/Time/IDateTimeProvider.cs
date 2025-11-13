namespace Application.Time;

/// <summary>
/// Provides access to the current UTC date and time.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    public DateTime UtcNow { get; }
}
