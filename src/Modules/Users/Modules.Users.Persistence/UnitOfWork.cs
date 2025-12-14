using Application.ServiceLifetimes;
using Modules.Users.Domain;

namespace Modules.Users.Persistence;

/// <summary>
/// Represents the user's module unit of work.
/// </summary>
public class UnitOfWork(UsersDbContext dbContext) : IUnitOfWork, IScoped
{
    /// <summary>
    /// Persists pending changes in the users database context.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the save operation.</param>
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}