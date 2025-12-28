using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Workspaces.Application.Workspaces.GetForTenant;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Workspaces.Endpoints.Workspaces.GetForTenant;

public sealed class GetWorkspacesForTenantEndpoint(ISender sender) : Endpoint
{
    [HttpGet(WorkspaceRoutes.GetForTenant, Name = nameof(GetWorkspacesForTenantEndpoint))]
    [ProducesResponseType(typeof(WorkspaceResponse[]), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get tenant workspaces",
        Description = "Retrieves workspaces owned by specific tenant identifier.",
        Tags = [WorkspaceRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var query = new GetWorkspacesForTenantQuery(tenantId);
        
        Result<WorkspaceResponse[]> result = await sender.Send(query, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) : Ok(result.Value);
    }
}
