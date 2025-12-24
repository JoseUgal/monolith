using Application.Messaging;

namespace Modules.Tenants.Application.Tenants.InviteMember;

/// <summary>
/// Represents the command for invite a new tenant member.
/// </summary>
/// <param name="TenantId">The tenant identifier.</param>
/// <param name="RequestedUserId">The requested user identifier.</param>
/// <param name="UserId">The user identifier.</param>
/// <param name="Role">The role.</param>
public sealed record InviteTenantMemberCommand(
    Guid TenantId,
    Guid RequestedUserId,
    Guid UserId,
    string Role
) : ICommand<Guid>;
