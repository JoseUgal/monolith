using Modules.Workspaces.Application.Workspaces.InviteMember;
using Modules.Workspaces.Domain;
using Modules.Workspaces.Domain.Workspaces;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.InviteMember;

internal sealed class InviteWorkspaceMemberCommandHandlerSut
{
    public Mock<IWorkspaceRepository> Repository { get; } = new(MockBehavior.Loose);
    
    public Mock<IUnitOfWork>  UnitOfWork { get; } = new(MockBehavior.Strict);
    
    public InviteWorkspaceMemberCommandHandler Handler { get; }

    public InviteWorkspaceMemberCommandHandlerSut()
    {
        Handler = new InviteWorkspaceMemberCommandHandler(
            Repository.Object, 
            UnitOfWork.Object
        );
    }

    public InviteWorkspaceMemberCommand ValidCommand() => new(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        "Member"
    );

    public void VerifyUnitOfWorkWasNotCalled()
    {
        UnitOfWork.Verify(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
    
    public void VerifyUnitOfWorkWasCalled()
    {
        UnitOfWork.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
