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
}
