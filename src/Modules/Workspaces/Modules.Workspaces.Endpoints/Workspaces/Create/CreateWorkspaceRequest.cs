namespace Modules.Workspaces.Endpoints.Workspaces.Create;

/// <summary>
/// Represents the request for creating a new workspace.
/// </summary>
/// <param name="Name">The name.</param>
public sealed record CreateWorkspaceRequest(
    string Name
);
