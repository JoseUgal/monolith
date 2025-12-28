using Domain.Errors;
using Domain.Results;
using FluentAssertions;
using Modules.Workspaces.Application.Workspaces.GetMembers;
using Modules.Workspaces.Domain.Workspaces;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.GetMembers;

public sealed class GetWorkspaceMembersQueryHandlerTests
{
    [Fact]
    public async Task Handle_WhenWorkspaceDoesNotExist_ReturnsNotFound_AndDoesNotQuerySql()
    {
        // Arrange
        var sut = new GetWorkspaceMembersQueryHandlerSut();

        sut.Repository.Setup(x =>
            x.ExistsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var query = new GetWorkspaceMembersQuery(Guid.NewGuid());
        
        // Act
        Result<WorkspaceMemberResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.NotFound);

        sut.Repository.Verify(x => 
            x.ExistsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        
        sut.Sql.Verify(x =>
            x.QueryAsync<WorkspaceMemberResponse>(It.IsAny<string>(), It.IsAny<object?>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Handle_WhenWorkspaceExists_QueriesSql_AndReturnsArray()
    {
        // Arrange
        var sut = new GetWorkspaceMembersQueryHandlerSut();

        WorkspaceMemberResponse[] responses =
        [
            sut.ValidResponse()
        ];
        
        sut.Repository.Setup(x =>
            x.ExistsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        sut.Sql.Setup(x =>
            x.QueryAsync<WorkspaceMemberResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync(responses);
        
        var query = new GetWorkspaceMembersQuery(Guid.NewGuid());
        
        // Act
        Result<WorkspaceMemberResponse[]> result = await sut.Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Length.Should().Be(responses.Length);
        result.Value.Should().BeEquivalentTo(responses);

        sut.Repository.Verify(x => 
                x.ExistsAsync(It.IsAny<WorkspaceId>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        
        sut.Sql.Verify(x =>
                x.QueryAsync<WorkspaceMemberResponse>(It.IsAny<string>(), It.IsAny<object?>()),
            Times.Once
        );
    }
}
