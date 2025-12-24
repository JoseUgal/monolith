namespace Modules.Tenants.Endpoints.Tenants.InviteMember;

/// <summary>
/// Represents the request for inviting a new tenant member.
/// </summary>
/// <param name="UserId">The user identifier.</param>
/// <param name="Role">The role.</param>
public sealed record InviteTenantMemberRequest(Guid UserId, string Role);
