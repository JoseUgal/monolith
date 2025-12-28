namespace Modules.Workspaces.Domain.Workspaces;


/// <summary>
/// Represents the workspace repository interface.
/// </summary>
public interface IWorkspaceRepository
{
    /// <summary>
    /// Adds the specified workspace to the repository.
    /// </summary>
    /// <param name="workspace">The workspace.</param>
    void Add(Workspace workspace);
    
    /// <summary>
    /// Gets the workspace with the specified identifier.
    /// </summary>
    /// <param name="workspaceId">The workspace identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The workspace with the specified identifier.</returns>
    /// <remarks>
    /// This method returns the workspace with its memberships.
    /// </remarks>
    Task<Workspace?> GetWithMembershipsAsync(WorkspaceId workspaceId, CancellationToken cancellationToken = default);
}
