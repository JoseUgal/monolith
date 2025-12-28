using Modules.Workspaces.Application.Workspaces.Create;
using Modules.Workspaces.Domain;
using Modules.Workspaces.Domain.Workspaces;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.Create;

internal sealed class CreateWorkspaceCommandHandlerSut
{
    public Mock<IWorkspaceRepository> Repository { get; } = new(MockBehavior.Loose);
    
    public Mock<IUnitOfWork> UnitOfWork { get; } = new(MockBehavior.Strict);

    public readonly CreateWorkspaceCommandHandler Handler;

    public CreateWorkspaceCommandHandlerSut()
    {
        Handler = new CreateWorkspaceCommandHandler(
            Repository.Object, 
            UnitOfWork.Object
        );
    }

    public CreateWorkspaceCommand ValidCommand() => new(
        Guid.NewGuid(),
        "My workspace",
        Guid.NewGuid()
    );
}
