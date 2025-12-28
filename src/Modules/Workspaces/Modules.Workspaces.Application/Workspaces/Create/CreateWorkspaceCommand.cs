using Application.Messaging;

namespace Modules.Workspaces.Application.Workspaces.Create;

/// <summary>
/// Represents the command for creating a new workspace.
/// </summary>
/// <param name="TenantId">The tenant identifier.</param>
/// <param name="Name">The name.</param>
/// <param name="UserId">The user identifier.</param>
public sealed record CreateWorkspaceCommand(
    Guid TenantId,
    string Name,
    Guid UserId
) : ICommand<Guid>;
