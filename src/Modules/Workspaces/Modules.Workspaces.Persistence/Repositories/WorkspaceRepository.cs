using Application.ServiceLifetimes;
using Microsoft.EntityFrameworkCore;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Persistence.Repositories;

/// <summary>
/// Represents the workspace repository.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class WorkspaceRepository(WorkspacesDbContext dbContext) : IWorkspaceRepository, IScoped
{
    /// <inheritdoc />
    public void Add(Workspace workspace) => dbContext.Set<Workspace>().Add(workspace);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(WorkspaceId workspaceId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Workspace>().AnyAsync(workspace => 
            workspace.Id == workspaceId, 
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<Workspace?> GetWithMembershipsAsync(WorkspaceId workspaceId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Workspace>()
            .Include(workspace => workspace.Memberships)
            .SingleOrDefaultAsync(workspace => 
                workspace.Id == workspaceId, 
                cancellationToken
            );
    }
}
