using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Application.Workspaces.AcceptInvitation;
using Modules.Workspaces.Domain.Tests.Common.Mothers;
using Modules.Workspaces.Domain.WorkspaceMemberships;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Application.Tests.Workspaces.AcceptInvitation;

public sealed class AcceptWorkspaceInvitationCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenWorkspaceDoesNotExist_ReturnsFailure_AndDoesNotSave()
    {
        // Arrange
        var sut = new AcceptWorkspaceInvitationCommandHandlerSut();

        AcceptWorkspaceInvitationCommand command = sut.ValidCommand() with { WorkspaceId = Guid.NewGuid() };

        sut.SetupRepositoryGetWithMembershipsAsync(null);
        
        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        
        sut.VerifyRepositoryGetWithMembershipsAsyncWasCalled();
        
        sut.VerifyUnitOfWorkSaveChangesAsyncWasNotCalled();
    }

    [Fact]
    public async Task Handle_WhenDomainReturnsFailure_PropagatesError_AndDoesNotSave()
    {
        // Arrange
        var sut = new AcceptWorkspaceInvitationCommandHandlerSut();

        Workspace workspace = WorkspaceMother.Create();
        
        AcceptWorkspaceInvitationCommand command = sut.ValidCommand() with
        {
            WorkspaceId = workspace.Id.Value,
            MembershipId = Guid.NewGuid()
        };
        
        sut.SetupRepositoryGetWithMembershipsAsync(workspace);

        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        
        sut.VerifyRepositoryGetWithMembershipsAsyncWasCalled();
        
        sut.VerifyUnitOfWorkSaveChangesAsyncWasNotCalled();
    }

    [Fact]
    public async Task Handle_WhenDomainReturnsSuccess_SavesChanges_Once_AndReturnsSuccess()
    {
        // Arrange
        var sut = new AcceptWorkspaceInvitationCommandHandlerSut();

        var memberId = Guid.NewGuid();
        
        Workspace workspace = WorkspaceMother.CreateWithMemberships(
            (memberId, WorkspaceMembershipRole.Member, WorkspaceMembershipStatus.Invited)    
        );
        
        WorkspaceMembership invited = workspace.Memberships.Single(m => m.UserId == memberId);
        
        var command = new AcceptWorkspaceInvitationCommand(
            WorkspaceId: workspace.Id.Value, 
            MembershipId: invited.Id.Value, 
            UserId: invited.UserId
        );
        
        sut.SetupRepositoryGetWithMembershipsAsync(workspace);
        
        sut.SetupUnitOfWorkSaveChangesAsyncSuccess();
        
        // Act
        Result result = await sut.Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        
        sut.VerifyRepositoryGetWithMembershipsAsyncWasCalled();
        
        sut.VerifyUnitOfWorkSaveChangesAsyncWasCalled();
    }
}
