using Application.ServiceLifetimes;
using Modules.Workspaces.Domain;

namespace Modules.Workspaces.Persistence;

/// <summary>
/// Represents the workspace's module unit of work.
/// </summary>
public class UnitOfWork(WorkspacesDbContext dbContext) : IUnitOfWork, IScoped
{
    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
