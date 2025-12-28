using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Application.Workspaces.GetById;

namespace Modules.Workspaces.Application.Tests.Workspaces.GetById;

public sealed class GetWorkspaceByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenWorkspaceDoesNotExist()
    {
        // Arrange
        var sut = new GetWorkspaceByIdQueryHandlerSut();
        
        var query = new GetWorkspaceByIdQuery(Guid.NewGuid());
        
        sut.SetupSqlReturnsNull();

        // Act
        Result<WorkspaceResponse> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);
        
        sut.VerifySqlQueryWasCalled();
    }

    [Fact]
    public async Task Handle_ShouldReturnWorkspace_WhenWorkspaceExists()
    {
        // Arrange
        var sut = new GetWorkspaceByIdQueryHandlerSut();
        
        var workspaceId = Guid.NewGuid();
        
        var query = new GetWorkspaceByIdQuery(workspaceId);

        WorkspaceResponse response = sut.ValidResponse(workspaceId);
        
        sut.SetupSqlReturnsWorkspace(response);

        // Act
        Result<WorkspaceResponse> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(workspaceId);
        result.Value.Name.Should().Be(response.Name);
        result.Value.TenantId.Should().Be(response.TenantId);
        
        sut.VerifySqlQueryWasCalled();
    }
}
