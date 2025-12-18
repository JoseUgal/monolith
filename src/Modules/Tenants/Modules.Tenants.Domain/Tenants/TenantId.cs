using Domain.Primitives;

namespace Modules.Tenants.Domain.Tenants;

/// <summary>
/// Represents the tenant identifier.
/// </summary>
/// <param name="Value">The value.</param>
public sealed record TenantId(Guid Value) : IEntityId;
