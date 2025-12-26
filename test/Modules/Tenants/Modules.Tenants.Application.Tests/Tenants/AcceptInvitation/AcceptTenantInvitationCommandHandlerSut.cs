using Modules.Tenants.Application.Tenants.AcceptInvitation;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.Tenants;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.AcceptInvitation;

internal sealed class AcceptTenantInvitationCommandHandlerSut
{
    public Mock<ITenantRepository> Repository { get; } = new(MockBehavior.Loose);
    
    public Mock<IUnitOfWork> UnitOfWorkMock { get; } = new(MockBehavior.Strict);

    public AcceptTenantInvitationCommandHandler Handler { get; }

    public AcceptTenantInvitationCommandHandlerSut()
    {
        Handler = new AcceptTenantInvitationCommandHandler(
            Repository.Object,
            UnitOfWorkMock.Object
        );
    }

    public AcceptTenantInvitationCommand ValidCommand() => new(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
    );

    public void SetupGetWithMembershipsReturnsTenant(Tenant tenant)
    {
        Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.Is<TenantId>(tenantId => tenantId == tenant.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(tenant);
    }

    
    public void SetupGetWithMembershipsReturnsNullable()
    {
        Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<TenantId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((Tenant?)null);
    }
    
    public void SetupUnitOfWork()
    {
        UnitOfWorkMock.Setup(x => 
            x.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).Returns(Task.CompletedTask);
    }
    
    public void VerifyPersistedNever()
    {
        UnitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
    
    public void VerifyPersistedOnce()
    {
        UnitOfWorkMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
