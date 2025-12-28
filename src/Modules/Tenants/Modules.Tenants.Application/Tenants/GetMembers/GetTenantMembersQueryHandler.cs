using Application.Messaging;
using Domain.Results;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;

namespace Modules.Tenants.Application.Tenants.GetMembers;

/// <summary>
/// Represents the <see cref="GetTenantMembersQuery"/> handler.
/// </summary>
internal sealed class GetTenantMembersQueryHandler(ITenantRepository repository) : IQueryHandler<GetTenantMembersQuery, TenantMemberResponse[]>
{
    /// <inheritdoc />
    public async Task<Result<TenantMemberResponse[]>> Handle(GetTenantMembersQuery request, CancellationToken cancellationToken)
    {
        var tenantId = new TenantId(request.TenantId);

        if (!await repository.ExistsAsync(tenantId, cancellationToken))
        {
            return Result.Failure<TenantMemberResponse[]>(
                TenantErrors.NotFound(tenantId)
            );
        }

        TenantMembership[] members = await repository.GetMembersAsync(tenantId, cancellationToken);

        return members.Select(member => new TenantMemberResponse
            {
                UserId = member.UserId,
                Role = member.MembershipRole.ToString().ToLowerInvariant(),
                Status = member.Status.ToString().ToLowerInvariant()
            }
        ).ToArray();
    }
}
