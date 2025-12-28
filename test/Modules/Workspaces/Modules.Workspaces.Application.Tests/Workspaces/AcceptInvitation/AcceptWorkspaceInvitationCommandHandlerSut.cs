using Modules.Workspaces.Application.Workspaces.AcceptInvitation;
using Modules.Workspaces.Domain;
using Modules.Workspaces.Domain.Workspaces;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.AcceptInvitation;

internal sealed class AcceptWorkspaceInvitationCommandHandlerSut
{
    public Mock<IWorkspaceRepository> Repository { get; } = new(MockBehavior.Loose);
    
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);
    
    public AcceptWorkspaceInvitationCommandHandler Handler { get; }

    public AcceptWorkspaceInvitationCommandHandlerSut()
    {
        Handler = new AcceptWorkspaceInvitationCommandHandler(
            Repository.Object, 
            UnitOfWork.Object
        );
    }

    public AcceptWorkspaceInvitationCommand ValidCommand() => new(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid()
    );

    public void SetupRepositoryGetWithMembershipsAsync(Workspace? workspace)
    {
        if (workspace == null)
        {
            Repository.Setup(x =>
                x.GetWithMembershipsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync((Workspace?)null);

            return;
        }
        
        Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.Is<WorkspaceId>(workspaceId => workspaceId == workspace.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(workspace);
    }

    public void SetupUnitOfWorkSaveChangesAsyncSuccess()
    {
        UnitOfWork.Setup(x => 
            x.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).Returns(Task.CompletedTask);
    }

    public void VerifyUnitOfWorkSaveChangesAsyncWasCalled()
    {
        UnitOfWork.Verify(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
    
    public void VerifyUnitOfWorkSaveChangesAsyncWasNotCalled()
    {
        UnitOfWork.Verify(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    public void VerifyRepositoryGetWithMembershipsAsyncWasCalled()
    {
        Repository.Verify(x =>
            x.GetWithMembershipsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}
