using Application.ServiceLifetimes;
using Application.Time;

namespace Infrastructure.Time;

/// <inheritdoc cref="IDateTimeProvider" />
internal sealed class DateTimeProvider : IDateTimeProvider, ISingleton
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
