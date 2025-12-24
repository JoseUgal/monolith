using Application.Messaging;
using Domain.Results;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Application.Tenants.InviteMember;

/// <summary>
/// Represents the <see cref="InviteTenantMemberCommand"/> handler.
/// </summary>
internal sealed class InviteTenantMemberCommandHandler(ITenantRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<InviteTenantMemberCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(InviteTenantMemberCommand command, CancellationToken cancellationToken)
    {
        var tenantId = new TenantId(command.TenantId);
        
        Tenant? tenant = await repository.GetWithMembershipsAsync(tenantId, cancellationToken);

        if (tenant == null)
        {
            return Result.Failure<Guid>(
                TenantErrors.NotFound(tenantId)
            );
        }

        if (!Enum.TryParse(command.Role, ignoreCase: true, out TenantRole role))
        {
            return Result.Failure<Guid>(
                TenantMembershipErrors.Role.IsInvalid
            );
        }

        Result<TenantMembership> invite = tenant.InviteMember(command.UserId, role);

        if (invite.IsFailure)
        {
            return Result.Failure<Guid>(invite.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return invite.Value.Id.Value;
    }
}
