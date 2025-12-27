using Application.ServiceLifetimes;
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
}
