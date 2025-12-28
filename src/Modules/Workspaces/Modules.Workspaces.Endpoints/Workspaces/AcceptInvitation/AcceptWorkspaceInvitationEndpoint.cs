using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Workspaces.Application.Workspaces.AcceptInvitation;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Workspaces.Endpoints.Workspaces.AcceptInvitation;

public sealed class AcceptWorkspaceInvitationEndpoint(ISender sender) : Endpoint
{
    [HttpPost(WorkspaceRoutes.AcceptInvitation, Name = nameof(AcceptWorkspaceInvitationEndpoint))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerOperation(
        Summary = "Accept workspace invitation",
        Description = "Accepts an invitation to join a workspace.",
        Tags = [WorkspaceRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid workspaceId, Guid membershipId, CancellationToken cancellationToken = default)
    {
        // TODO: Gets the user identifier from CurrentUser.
        var userId = Guid.Parse("7c2fd1d9-db1c-443a-a7e9-f2d106d6a04e");

        var command = new AcceptWorkspaceInvitationCommand(
            workspaceId,
            membershipId,
            userId
        );
        
        Result result = await sender.Send(command, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) :  NoContent();
    }
}
