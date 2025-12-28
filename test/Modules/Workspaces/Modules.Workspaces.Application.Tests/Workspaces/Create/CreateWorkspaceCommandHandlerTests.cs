using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Application.Workspaces.Create;
using Modules.Workspaces.Domain.Workspaces;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.Create;

public sealed class CreateWorkspaceCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenNameIsInvalid_ReturnsFailure_AndDoesNotPersist()
    {
        // Arrange
        var sut = new CreateWorkspaceCommandHandlerSut();

        CreateWorkspaceCommand command = sut.ValidCommand() with
        {
            Name = ""
        };

        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);
        
        sut.UnitOfWork.Verify(x => 
            x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
        
        sut.Repository.Verify(x =>
            x.Add(It.IsAny<Workspace>()), 
            Times.Never    
        );
    }

    [Fact]
    public async Task Handle_WhenNameIsValid_PersistsWorkspace_AndReturnsWorkspaceId()
    {
        // Arrange
        var sut = new CreateWorkspaceCommandHandlerSut();

        CreateWorkspaceCommand command = sut.ValidCommand();

        sut.Repository.Setup(x =>
            x.Add(It.IsAny<Workspace>())
        );

        sut.UnitOfWork.Setup(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).Returns(Task.CompletedTask);
        
        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        
        result.IsSuccess.Should().BeTrue();
        
        sut.UnitOfWork.Verify(x => 
            x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
        
        sut.Repository.Verify(x =>
            x.Add(It.IsAny<Workspace>()), 
            Times.Once    
        );
    }
}
