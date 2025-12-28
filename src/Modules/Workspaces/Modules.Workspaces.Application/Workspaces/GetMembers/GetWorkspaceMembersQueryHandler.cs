using Application.Data;
using Application.Messaging;
using Domain.Results;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Application.Workspaces.GetMembers;

/// <summary>
/// Represents the <see cref="GetWorkspaceMembersQuery"/> handler.
/// </summary>
internal sealed class GetWorkspaceMembersQueryHandler(IWorkspaceRepository repository, ISqlQueryExecutor sqlQueryExecutor)  : IQueryHandler<GetWorkspaceMembersQuery, WorkspaceMemberResponse[]>
{
    /// <inheritdoc />
    public async Task<Result<WorkspaceMemberResponse[]>> Handle(GetWorkspaceMembersQuery query, CancellationToken cancellationToken)
    {
        var workspaceId = new WorkspaceId(query.WorkspaceId);
        
        if (!await repository.ExistsAsync(workspaceId, cancellationToken))
        {
            return Result.Failure<WorkspaceMemberResponse[]>(
                WorkspaceErrors.NotFound(workspaceId)
            );
        }
        
        IEnumerable<WorkspaceMemberResponse> responses = await sqlQueryExecutor.QueryAsync<WorkspaceMemberResponse>(
            """
                SELECT 
                    wm.id, wm.user_id, wm.role, wm.status
                FROM workspaces.workspace_memberships wm
                WHERE wm.workspace_id = @workspaceId
                ORDER BY wm.created_on_utc
            """,
            new { workspaceId = workspaceId.Value }
        );

        return responses.ToArray();
    }
}
