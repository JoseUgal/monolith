using Application.Data;
using Modules.Workspaces.Application.Workspaces.GetMembers;
using Modules.Workspaces.Domain.WorkspaceMemberships;
using Modules.Workspaces.Domain.Workspaces;
using Moq;

namespace Modules.Workspaces.Application.Tests.Workspaces.GetMembers;

internal sealed class GetWorkspaceMembersQueryHandlerSut
{
    public Mock<IWorkspaceRepository> Repository { get; } = new(MockBehavior.Loose);
    
    public Mock<ISqlQueryExecutor> Sql { get; } = new(MockBehavior.Strict);
    
    public GetWorkspaceMembersQueryHandler Handler { get; }

    public GetWorkspaceMembersQueryHandlerSut()
    {
        Handler = new GetWorkspaceMembersQueryHandler(
            Repository.Object, 
            Sql.Object
        );
    }

    public WorkspaceMemberResponse ValidResponse() => new()
    {
        Id = Guid.NewGuid(),
        UserId = Guid.NewGuid(),
        Status = nameof(WorkspaceMembershipStatus.Active),
        Role = nameof(WorkspaceMembershipRole.Member)
    };
}
