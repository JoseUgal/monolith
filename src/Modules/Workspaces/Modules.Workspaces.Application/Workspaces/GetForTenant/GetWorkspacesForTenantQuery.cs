using Application.Messaging;

namespace Modules.Workspaces.Application.Workspaces.GetForTenant;


/// <summary>
/// Represents the query for getting a tenant's workspaces.
/// </summary>
/// <param name="TenantId">The tenant identifier.</param>
public sealed record GetWorkspacesForTenantQuery(Guid TenantId) : IQuery<WorkspaceResponse[]>;
