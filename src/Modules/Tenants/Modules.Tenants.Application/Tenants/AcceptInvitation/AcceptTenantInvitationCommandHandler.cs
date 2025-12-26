using Application.Messaging;
using Domain.Results;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Application.Tenants.AcceptInvitation;

/// <summary>
/// Represents the <see cref="AcceptTenantInvitationCommand"/> handler.
/// </summary>
internal sealed class AcceptTenantInvitationCommandHandler(ITenantRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<AcceptTenantInvitationCommand>
{
    /// <inheritdoc />
    public async Task<Result> Handle(AcceptTenantInvitationCommand command, CancellationToken cancellationToken)
    {
        var tenantId = new TenantId(command.TenantId);
        
        Tenant? tenant = await repository.GetWithMembershipsAsync(tenantId, cancellationToken);

        if (tenant == null)
        {
            return Result.Failure(
                TenantErrors.NotFound(tenantId)
            );
        }

        var membershipId = new TenantMembershipId(command.MembershipId);

        Result accept = tenant.AcceptInvitation(membershipId, command.UserId);

        if (accept.IsFailure)
        {
            return Result.Failure(accept.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
