using Application.Messaging;

namespace Modules.Tenants.Application.Tenants.AcceptInvitation;

/// <summary>
/// Represents the command for accepting a tenant invitation.
/// </summary>
/// <param name="TenantId">The tenant identifier.</param>
/// <param name="MembershipId">The tenant membership identifier.</param>
/// <param name="UserId">The current user identifier.</param>
public sealed record AcceptTenantInvitationCommand(
    Guid TenantId, 
    Guid MembershipId,
    Guid UserId
) : ICommand;
