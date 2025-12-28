using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Workspaces.Application.Workspaces.Create;
using Modules.Workspaces.Endpoints.Workspaces.GetById;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Workspaces.Endpoints.Workspaces.Create;

public sealed class CreateWorkspaceEndpoint(ISender sender) : Endpoint
{
    [HttpPost(WorkspaceRoutes.Create, Name = nameof(CreateWorkspaceEndpoint))]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Create a new workspace",
        Description = "Creates a new workspace owned by the currently authenticated user.",
        Tags = [WorkspaceRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid tenantId, CreateWorkspaceRequest request, CancellationToken cancellation)
    {
        // TODO: Gets the user identifier from CurrentUser.
        var userId = Guid.Parse("7c2fd1d9-db1c-443a-a7e9-f2d106d6a04e");
        
        var command = new CreateWorkspaceCommand(
            tenantId,
            request.Name,
            userId
        );
        
        Result<Guid> result = await sender.Send(command, cancellation);

        return result.IsFailure ? this.HandleFailure(result) : CreatedAtRoute(
            nameof(GetWorkspaceByIdEndpoint),
            new { workspaceId = result.Value },
            result.Value
        );
    }
}
