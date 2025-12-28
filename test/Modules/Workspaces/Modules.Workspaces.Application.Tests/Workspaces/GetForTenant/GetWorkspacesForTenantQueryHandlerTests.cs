using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Application.Workspaces.GetForTenant;

namespace Modules.Workspaces.Application.Tests.Workspaces.GetForTenant;

public sealed class GetWorkspacesForTenantQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnEmptyArray_WhenTenantHasNoWorkspaces()
    {
        // Arrange
        var sut = new GetWorkspacesForTenantQueryHandlerSut();

        var query = new GetWorkspacesForTenantQuery(Guid.NewGuid());

        sut.SetupSqlReturnsEmptyArray();
        
        // Act
        Result<WorkspaceResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
        result.Value.Should().NotBeNull();

        sut.VerifySqlQueryWasCalled();
    }
    
    [Fact]
    public async Task Handle_ShouldReturnWorkspaces_WhenTenantHasWorkspaces()
    {
        // Arrange
        var sut = new GetWorkspacesForTenantQueryHandlerSut();
        
        WorkspaceResponse[] responses =
        [
            sut.ValidResponse("Workspace 1"),
            sut.ValidResponse("Workspace 2")
        ];
        
        sut.SetupSqlReturnsWorkspaces(responses);

        var query = new GetWorkspacesForTenantQuery(Guid.NewGuid());
        
        // Act
        Result<WorkspaceResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(responses.Length);
        result.Value.Select(x => x.Name).Should().Contain(["Workspace 1", "Workspace 2"]);
        
        sut.VerifySqlQueryWasCalled();
    }
}
