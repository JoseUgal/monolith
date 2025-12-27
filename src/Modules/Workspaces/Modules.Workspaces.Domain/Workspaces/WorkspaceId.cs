using Domain.Primitives;

namespace Modules.Workspaces.Domain.Workspaces;

/// <summary>
/// Represents the workspace identifier.
/// </summary>
/// <param name="Value">The value.</param>
public sealed record WorkspaceId(Guid Value) : IEntityId;
