using Domain.Primitives;

namespace Modules.Workspaces.Domain.WorkspaceMemberships;

/// <summary>
/// Represents the workspace membership identifier.
/// </summary>
/// <param name="Value">The value.</param>
public sealed record WorkspaceMembershipId(Guid Value) : IEntityId;
