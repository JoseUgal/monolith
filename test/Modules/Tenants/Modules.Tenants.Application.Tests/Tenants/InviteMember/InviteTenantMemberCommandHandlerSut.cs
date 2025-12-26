using Modules.Tenants.Application.Tenants.InviteMember;
using Modules.Tenants.Domain;
using Modules.Tenants.Domain.TenantMemberships;
using Modules.Tenants.Domain.Tenants;
using Moq;

namespace Modules.Tenants.Application.Tests.Tenants.InviteMember;

internal class InviteTenantMemberCommandHandlerSut
{
    public Mock<ITenantRepository> Repository { get; } = new(MockBehavior.Loose);
    
    public Mock<IUnitOfWork> UnitOfWorkMock { get; } = new(MockBehavior.Strict);

    public InviteTenantMemberCommandHandler Handler { get; }

    public InviteTenantMemberCommandHandlerSut()
    {
        Handler = new InviteTenantMemberCommandHandler(
            Repository.Object,
            UnitOfWorkMock.Object
        );
    }

    public InviteTenantMemberCommand ValidCommand() => new(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        nameof(TenantRole.Member)
    );
    
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
