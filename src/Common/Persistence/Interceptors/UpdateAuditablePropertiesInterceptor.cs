using Application.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Constants;

namespace Persistence.Interceptors;

/// <summary>
/// Represents the interceptor for updating auditable shadow property values.
/// </summary>
public sealed class UpdateAuditablePropertiesInterceptor(ISystemTime systemTime) : SaveChangesInterceptor
{
    /// <summary>
    /// Updates auditable shadow properties on tracked entities before saving: sets `CreatedOnUtc` for added entries and `ModifiedOnUtc` for modified entries using the provided system time, then continues the save pipeline.
    /// </summary>
    /// <param name="eventData">Contextual data for the save operation; if <see cref="DbContextEventData.Context"/> is null the method delegates to the base implementation without changes.</param>
    /// <param name="result">The current interception result for the save operation.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the operation to complete.</param>
    /// <returns>An <see cref="InterceptionResult{Int32}"/> produced by the base <see cref="SavingChangesAsync(DbContextEventData,InterceptionResult{int},CancellationToken)"/> after auditable properties have been updated.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        DateTime utcNow = systemTime.UtcNow;

        foreach (EntityEntry entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                TryUpdateProperty(entry, AuditableProperties.CreatedOnUtc, utcNow);
            }

            if (entry.State == EntityState.Modified)
            {
                TryUpdateProperty(entry, AuditableProperties.ModifiedOnUtc, utcNow);
            }
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Sets the CurrentValue of the named property on the given entity entry if that property exists.
    /// </summary>
    /// <param name="entry">The entity entry whose property should be updated.</param>
    /// <param name="propertyName">The metadata name of the property to update.</param>
    /// <param name="value">The value to assign to the property.</param>
    private static void TryUpdateProperty(EntityEntry entry, string propertyName, object value)
    {
        IProperty? property = entry.Metadata.FindProperty(propertyName);

        if (property is null)
        {
            return;
        }
        
        entry.Property(propertyName).CurrentValue = value;
    }
}