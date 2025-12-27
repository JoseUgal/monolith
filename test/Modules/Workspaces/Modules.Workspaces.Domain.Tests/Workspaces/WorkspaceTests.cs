using Domain.Results;
using FluentAssertions;
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
        Result<Workspace> result = Workspace.Create(name, ownerId);

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
        Workspace workspace = Workspace.Create(name, ownerId).Value;

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
        Workspace workspace = Workspace.Create(name, ownerId).Value;
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
        Workspace w1 = Workspace.Create(name, Guid.NewGuid()).Value;
        Workspace w2 = Workspace.Create(name, Guid.NewGuid()).Value;

        // Assert
        w1.Id.Should().NotBe(w2.Id);
    }
}
