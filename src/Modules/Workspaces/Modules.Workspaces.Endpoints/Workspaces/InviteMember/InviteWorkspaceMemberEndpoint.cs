using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Workspaces.Application.Workspaces.InviteMember;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Workspaces.Endpoints.Workspaces.InviteMember;

public sealed class InviteWorkspaceMemberEndpoint(ISender sender) : Endpoint
{
    [HttpPost(WorkspaceRoutes.InviteMember, Name = nameof(InviteWorkspaceMemberEndpoint))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Invite a new workspace member",
        Description = "Invites a new workspace member to the specified workspace.",
        Tags = [WorkspaceRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid workspaceId, InviteWorkspaceMemberRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Gets the user identifier from CurrentUser.
        var userId = Guid.Parse("7c2fd1d9-db1c-443a-a7e9-f2d106d6a04e");
        
        var command = new InviteWorkspaceMemberCommand(
            workspaceId,
            userId,
            request.UserId,
            request.Role
        );
        
        Result<Guid> result = await sender.Send(command, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) :  NoContent();
    }
}
