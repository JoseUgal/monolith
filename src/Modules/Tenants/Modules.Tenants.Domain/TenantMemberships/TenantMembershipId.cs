using Domain.Primitives;

namespace Modules.Tenants.Domain.TenantMemberships;

/// <summary>
/// Represents the tenant membership identifier.
/// </summary>
/// <param name="Value">The value.</param>
public sealed record TenantMembershipId(Guid Value) : IEntityId;
