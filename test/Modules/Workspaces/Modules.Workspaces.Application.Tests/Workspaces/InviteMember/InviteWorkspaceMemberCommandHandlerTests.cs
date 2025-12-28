using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Application.Workspaces.InviteMember;
using Modules.Workspaces.Domain.Tests.Common.Mothers;
using Modules.Workspaces.Domain.Workspaces;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.InviteMember;

public sealed class InviteWorkspaceMemberCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenWorkspaceDoesNotExist_ReturnsNotFound_AndDoesNotSave()
    {
        // Arrange
        var sut = new InviteWorkspaceMemberCommandHandlerSut();
        
        InviteWorkspaceMemberCommand command = sut.ValidCommand() with { WorkspaceId = Guid.NewGuid() };

        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync((Workspace?)null);

        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        
        sut.VerifyUnitOfWorkWasNotCalled();
    }
    
    [Fact]
    public async Task Handle_WhenRoleIsInvalid_ReturnsFailure_AndDoesNotSave()
    {
        // Arrange
        var sut = new InviteWorkspaceMemberCommandHandlerSut();
        
        InviteWorkspaceMemberCommand command = sut.ValidCommand() with { Role = "invalid" };

        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(WorkspaceMother.Create());

        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Failure);
        
        sut.VerifyUnitOfWorkWasNotCalled();
    }

    [Fact]
    public async Task Handle_WhenRoleIsOwner_ReturnsFailure_AndDoesNotSave()
    {
        // Arrange
        var sut = new InviteWorkspaceMemberCommandHandlerSut();
        
        InviteWorkspaceMemberCommand command = sut.ValidCommand() with { Role = "Owner" };

        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(WorkspaceMother.Create());
        
        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);
        
        sut.VerifyUnitOfWorkWasNotCalled();
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyMember_ReturnsConflict_AndDoesNotSave()
    {
        // Arrange
        var sut = new InviteWorkspaceMemberCommandHandlerSut();
        
        var ownerId = Guid.NewGuid();
        
        InviteWorkspaceMemberCommand command = sut.ValidCommand() with { UserId = ownerId };

        Workspace tenant = WorkspaceMother.Create(ownerId: ownerId);
        
        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(tenant);
        
        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Conflict);
        
        sut.VerifyUnitOfWorkWasNotCalled();
    }

    [Fact]
    public async Task Handle_WhenValid_InvitesMember_SavesChanges_AndMembershipIsInvited()
    {
        // Arrange
        var sut = new InviteWorkspaceMemberCommandHandlerSut();

        InviteWorkspaceMemberCommand command = sut.ValidCommand();

        sut.Repository.Setup(x =>
            x.GetWithMembershipsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(WorkspaceMother.Create());
        
        sut.UnitOfWork.Setup(x =>
            x.SaveChangesAsync(It.IsAny<CancellationToken>())    
        ).Returns(Task.CompletedTask);
        
        // Act
        Result<Guid> result = await sut.Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        
        sut.VerifyUnitOfWorkWasCalled();
    }
}
