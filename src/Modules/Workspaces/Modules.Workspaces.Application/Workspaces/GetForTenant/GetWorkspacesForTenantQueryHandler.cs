using Application.Data;
using Application.Messaging;
using Domain.Results;

namespace Modules.Workspaces.Application.Workspaces.GetForTenant;

/// <summary>
/// Represents the <see cref="GetWorkspacesForTenantQuery"/> handler.
/// </summary>
internal class GetWorkspacesForTenantQueryHandler(ISqlQueryExecutor sqlQueryExecutor) : IQueryHandler<GetWorkspacesForTenantQuery, WorkspaceResponse[]>
{
    /// <inheritdoc />
    public async Task<Result<WorkspaceResponse[]>> Handle(GetWorkspacesForTenantQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<WorkspaceResponse> responses = await sqlQueryExecutor.QueryAsync<WorkspaceResponse>(
            """
                SELECT 
                    id, name
                FROM workspaces.workspaces
                WHERE tenant_id = @tenantId
            """,
            new { tenantId = query.TenantId }
        );

        return responses.ToArray();
    }
}
