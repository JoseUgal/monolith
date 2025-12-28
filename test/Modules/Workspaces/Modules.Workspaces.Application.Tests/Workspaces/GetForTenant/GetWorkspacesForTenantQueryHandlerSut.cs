using Application.Data;
using Modules.Workspaces.Application.Workspaces.GetForTenant;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.GetForTenant;

internal sealed class GetWorkspacesForTenantQueryHandlerSut
{
    public Mock<ISqlQueryExecutor> Sql { get; } = new(MockBehavior.Strict);

    public GetWorkspacesForTenantQueryHandler Handler { get; }

    public GetWorkspacesForTenantQueryHandlerSut()
    {
        Handler = new GetWorkspacesForTenantQueryHandler(Sql.Object);
    }
    
    public void SetupSqlReturnsEmptyArray()
    {
        Sql.Setup(x =>
            x.QueryAsync<WorkspaceResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync([]);
    }

    public void SetupSqlReturnsWorkspaces(WorkspaceResponse[] responses)
    {
        Sql.Setup(x =>
            x.QueryAsync<WorkspaceResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync(responses);
    }
    
    public void VerifySqlQueryWasCalled()
    {
        Sql.Verify(x =>
                x.QueryAsync<WorkspaceResponse>(It.IsAny<string>(), It.IsAny<object?>()),
            Times.Once
        );
    }

    public WorkspaceResponse ValidResponse(string name) => new()
    {
        Id = Guid.NewGuid(),
        Name = name
    };
}
