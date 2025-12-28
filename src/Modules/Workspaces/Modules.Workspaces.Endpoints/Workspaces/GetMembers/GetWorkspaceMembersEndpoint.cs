using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Workspaces.Application.Workspaces.GetMembers;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Workspaces.Endpoints.Workspaces.GetMembers;

public sealed class GetWorkspaceMembersEndpoint(ISender sender) : Endpoint
{
    [HttpGet(WorkspaceRoutes.GetMembers, Name = nameof(GetWorkspaceMembersEndpoint))]
    [ProducesResponseType(typeof(WorkspaceMemberResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get workspace members",
        Description = "Retrieves workspace members.",
        Tags = [WorkspaceRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        var query = new GetWorkspaceMembersQuery(workspaceId);
        
        Result<WorkspaceMemberResponse[]> result = await sender.Send(query, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) : Ok(result.Value);
    }
}
