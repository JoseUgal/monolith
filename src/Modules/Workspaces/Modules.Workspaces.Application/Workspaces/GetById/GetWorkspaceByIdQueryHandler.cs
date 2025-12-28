using Application.Data;
using Application.Messaging;
using Domain.Results;
using Modules.Workspaces.Domain.Workspaces;

namespace Modules.Workspaces.Application.Workspaces.GetById;

/// <summary>
/// Represents the <see cref="GetWorkspaceByIdQuery"/> handler.
/// </summary>
internal sealed class GetWorkspaceByIdQueryHandler(ISqlQueryExecutor sqlQueryExecutor) : IQueryHandler<GetWorkspaceByIdQuery, WorkspaceResponse>
{
    /// <inheritdoc />
    public async Task<Result<WorkspaceResponse>> Handle(GetWorkspaceByIdQuery query, CancellationToken cancellationToken)
    {
        var workspaceId = new WorkspaceId(query.WorkspaceId);

        WorkspaceResponse? workspace = await sqlQueryExecutor.FirstOrDefaultAsync<WorkspaceResponse>(
            """
                SELECT id, name, tenant_id
                FROM workspaces.workspaces
                WHERE id = @workspaceId
            """,
            new { workspaceId = workspaceId.Value }
        );

        if (workspace == null)
        {
            return Result.Failure<WorkspaceResponse>(
                WorkspaceErrors.NotFound(workspaceId)
            );
        }

        return workspace;
    }
}
