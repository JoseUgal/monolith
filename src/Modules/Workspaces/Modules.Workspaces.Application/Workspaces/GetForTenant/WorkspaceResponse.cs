namespace Modules.Workspaces.Application.Workspaces.GetForTenant;

/// <summary>
/// Represents the workspace response.
/// </summary>
public sealed class WorkspaceResponse
{
    /// <summary>
    /// Gets the identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
