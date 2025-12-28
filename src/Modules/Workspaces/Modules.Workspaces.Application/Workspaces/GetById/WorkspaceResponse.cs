namespace Modules.Workspaces.Application.Workspaces.GetById;

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
    /// Gets the tenant identifier.
    /// </summary>
    public Guid TenantId { get; init; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; init; } = string.Empty;
}
