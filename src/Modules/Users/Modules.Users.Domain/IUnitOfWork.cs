namespace Modules.Users.Domain;

/// <summary>
/// Represents the users module unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <summary>
/// Persists all pending changes tracked by the unit of work.
/// </summary>
/// <param name="cancellationToken">A token to cancel the save operation.</param>
/// <returns>A task that completes when all pending changes have been saved.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}