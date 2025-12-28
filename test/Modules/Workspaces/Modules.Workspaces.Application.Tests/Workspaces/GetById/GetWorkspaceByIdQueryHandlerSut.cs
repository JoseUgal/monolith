using Application.Data;
using Modules.Workspaces.Application.Workspaces.GetById;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.GetById;

internal sealed class GetWorkspaceByIdQueryHandlerSut
{
    public Mock<ISqlQueryExecutor> Sql { get; } = new(MockBehavior.Strict);
    
    public GetWorkspaceByIdQueryHandler Handler { get; }

    public GetWorkspaceByIdQueryHandlerSut()
    {
        Handler = new GetWorkspaceByIdQueryHandler(
            Sql.Object
        );
    }
    
    public void SetupSqlReturnsWorkspace(WorkspaceResponse response)
    {
        Sql.Setup(x =>
            x.FirstOrDefaultAsync<WorkspaceResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync(response);
    }

    public void SetupSqlReturnsNull()
    {
        Sql.Setup(x =>
            x.FirstOrDefaultAsync<WorkspaceResponse>(It.IsAny<string>(), It.IsAny<object?>())
        ).ReturnsAsync((WorkspaceResponse?)null);
    }

    public void VerifySqlQueryWasCalled()
    {
        Sql.Verify(x =>
                x.FirstOrDefaultAsync<WorkspaceResponse>(It.IsAny<string>(), It.IsAny<object?>()),
            Times.Once
        );
    }

    public WorkspaceResponse ValidResponse(Guid? id = null, string? name = null, Guid? tenantId = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = name ?? "Workspace 1",
        TenantId = tenantId ?? Guid.NewGuid()
    };
}
