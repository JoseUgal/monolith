using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Domain.Tests.Common.Mothers;
using Modules.Workspaces.Domain.WorkspaceMemberships;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Domain.Tests.Workspaces;

public sealed class WorkspaceTests
{
    [Fact]
    public void Create_Valid_ReturnsSuccessAndCreatesWorkspace()
    {
        // Arrange
        WorkspaceName name = WorkspaceName.Create("My workspace").Value;

        var ownerId = Guid.NewGuid();

        // Act
        Result<Workspace> result = Workspace.Create(Guid.NewGuid(), name, ownerId);

        // Assert
        result.IsSuccess.Should().BeTrue();

        Workspace workspace = result.Value;
        workspace.Should().NotBeNull();

        workspace.Id.Value.Should().NotBe(Guid.Empty);
        workspace.Name.Value.Should().Be("My workspace");
    }

    [Fact]
    public void Create_Valid_AddsSingleOwnerMembership()
    {
        // Arrange
        WorkspaceName name = WorkspaceName.Create("Team A").Value;
        var ownerId = Guid.NewGuid();

        // Act
        Workspace workspace = Workspace.Create(Guid.NewGuid(), name, ownerId).Value;

        // Assert
        workspace.Memberships.Should().HaveCount(1);
    }

    [Fact]
    public void Create_Valid_OwnerMembershipHasExpectedValues()
    {
        // Arrange
        WorkspaceName name = WorkspaceName.Create("Team A").Value;
        var ownerId = Guid.NewGuid();

        // Act
        Workspace workspace = Workspace.Create(Guid.NewGuid(), name, ownerId).Value;
        WorkspaceMembership membership = workspace.Memberships.Single();

        // Assert
        membership.UserId.Should().Be(ownerId);
        membership.Role.Should().Be(WorkspaceMembershipRole.Owner);
        membership.Status.Should().Be(WorkspaceMembershipStatus.Active);

        membership.WorkspaceId.Should().Be(workspace.Id);
        membership.Id.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_TwoWorkspaces_ShouldHaveDifferentIds()
    {
        // Arrange
        WorkspaceName name = WorkspaceName.Create("Same name").Value;

        // Act
        Workspace w1 = Workspace.Create(Guid.NewGuid(), name, Guid.NewGuid()).Value;
        Workspace w2 = Workspace.Create(Guid.NewGuid(), name, Guid.NewGuid()).Value;

        // Assert
        w1.Id.Should().NotBe(w2.Id);
    }

    [Fact]
    public void InviteMember_Should_ReturnConflict_When_RoleIsOwner()
    {
        // Arrange
        Workspace workspace = WorkspaceMother.Create();
        
        var userId = Guid.NewGuid();

        // Act
        Result<WorkspaceMembership> result = workspace.InviteMember(userId, WorkspaceMembershipRole.Owner);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceErrors.OwnerAlreadyExist);
    }

    [Fact]
    public void InviteMember_Should_ReturnConflict_When_UserAlreadyMember()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        Workspace workspace = WorkspaceMother.CreateWithMemberships(
            (memberId, WorkspaceMembershipRole.Member, WorkspaceMembershipStatus.Active)
        );
        
        // Act
        Result<WorkspaceMembership> result = workspace.InviteMember(memberId, WorkspaceMembershipRole.Member);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceErrors.MemberAlreadyExist);
    }
    
    [Fact]
    public void InviteMember_Should_CreateInvitedMembership_When_UserIsNotMember()
    {
        // Arrange
        Workspace workspace = WorkspaceMother.Create();
        
        var userId = Guid.NewGuid();
        
        // Act
        Result<WorkspaceMembership> result = workspace.InviteMember(userId, WorkspaceMembershipRole.Member);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        workspace.Memberships.Should().HaveCount(2);
    }

    [Fact]
    public void InviteMember_Should_SetCorrectRole()
    {
        // Arrange
        Workspace workspace = WorkspaceMother.Create();

        WorkspaceMembershipRole role = WorkspaceMembershipRole.Admin;
        
        // Act
        Result<WorkspaceMembership> result = workspace.InviteMember(Guid.NewGuid(), role);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Role.Should().Be(role);
    }

    [Fact]
    public void InviteMember_Should_SetStatusAsInvited()
    {
        // Arrange
        Workspace workspace = WorkspaceMother.Create();
        
        // Act
        Result<WorkspaceMembership> result = workspace.InviteMember(Guid.NewGuid(), WorkspaceMembershipRole.Member);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(WorkspaceMembershipStatus.Invited);
    }

    [Fact]
    public void AcceptInvitation_Should_ActivateMembership_When_StatusIsInvited()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        Workspace workspace = WorkspaceMother.CreateWithMemberships(
            (memberId, WorkspaceMembershipRole.Member, WorkspaceMembershipStatus.Invited)
        );
        
        WorkspaceMembership invited = workspace.Memberships.Single(x => x.UserId == memberId);

        // Act
        Result result = workspace.AcceptInvitation(invited.Id, memberId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        invited.Status.Should().Be(WorkspaceMembershipStatus.Active);
    }

    [Fact]
    public void AcceptInvitation_Should_ReturnNotFound_When_MembershipDoesNotExist()
    {
        // Arrange
        Workspace workspace = WorkspaceMother.Create();

        var membershipId = new WorkspaceMembershipId(Guid.NewGuid());
        
        var userId = Guid.NewGuid();

        // Act
        Result result = workspace.AcceptInvitation(membershipId, userId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceMembershipErrors.NotFound(membershipId));
    }

    [Fact]
    public void AcceptInvitation_Should_ReturnConflict_When_MembershipIsAlreadyActive()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        Workspace workspace = WorkspaceMother.CreateWithMemberships(
            (memberId, WorkspaceMembershipRole.Member, WorkspaceMembershipStatus.Active)
        );
        
        WorkspaceMembership activated = workspace.Memberships.Single(x => x.UserId == memberId);

        // Act
        Result result = workspace.AcceptInvitation(activated.Id, memberId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceMembershipErrors.Invitation.AlreadyAccepted);
    }

    [Fact]
    public void AcceptInvitation_Should_ReturnForbidden_When_InvitationDoesNotBelongToUser()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        Workspace workspace = WorkspaceMother.CreateWithMemberships(
            (memberId, WorkspaceMembershipRole.Member, WorkspaceMembershipStatus.Invited)
        );
        
        WorkspaceMembership activated = workspace.Memberships.Single(x => x.UserId == memberId);

        // Act
        Result result = workspace.AcceptInvitation(activated.Id, Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceMembershipErrors.Invitation.NotForUser);
    }

    [Fact]
    public void Workspace_Should_Not_Allow_DuplicateMemberships_ForSameUser()
    {
        // Arrange
        var memberId = Guid.NewGuid();

        Workspace workspace = WorkspaceMother.CreateWithMemberships(
            (memberId, WorkspaceMembershipRole.Member, WorkspaceMembershipStatus.Invited)
        );

        // Act
        Result<WorkspaceMembership> result = workspace.InviteMember(memberId, WorkspaceMembershipRole.Member);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(WorkspaceErrors.MemberAlreadyExist);
    }

    [Fact]
    public void Workspace_Memberships_Should_Not_Be_Modifiable_Via_ExposedCollectionReference()
    {
        // Arrange
        Workspace workspace = WorkspaceMother.Create();
        
        int initialCount = workspace.Memberships.Count;
        
        // Act
        var memberships = workspace.Memberships.ToList();
        
        memberships.Add(WorkspaceMembership.Invite(workspace.Id, Guid.NewGuid(), WorkspaceMembershipRole.Member));
        
        // Assert
        workspace.Memberships.Should().HaveCount(initialCount);
        memberships.Should().HaveCount(initialCount + 1);
    }
}
