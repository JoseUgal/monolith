using Modules.Tenants.Application.Tenants.GetMembers;
using Modules.Tenants.Domain.Tenants;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.GetMembers;

internal sealed class GetTenantMembersQueryHandlerSut
{
    public Mock<ITenantRepository> Repository { get; } = new(MockBehavior.Default);

    public GetTenantMembersQueryHandler Handler { get; }

    public GetTenantMembersQueryHandlerSut()
    {
        Handler = new GetTenantMembersQueryHandler(Repository.Object);
    }

    public GetTenantMembersQuery ValidQuery() => new(Guid.NewGuid());
}
