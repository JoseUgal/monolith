using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Workspaces.Application.Workspaces.GetById;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Workspaces.Endpoints.Workspaces.GetById;

public sealed class GetWorkspaceByIdEndpoint(ISender sender) : Endpoint
{
    [HttpGet(WorkspaceRoutes.GetById, Name = nameof(GetWorkspaceByIdEndpoint))]
    [ProducesResponseType(typeof(WorkspaceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get workspace by id",
        Description = "Retrieves a workspace by its identifier.",
        Tags = [WorkspaceRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var query = new GetWorkspaceByIdQuery(workspaceId);
        
         Result<WorkspaceResponse> result = await sender.Send(query, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) : Ok(result.Value);
    }
}
