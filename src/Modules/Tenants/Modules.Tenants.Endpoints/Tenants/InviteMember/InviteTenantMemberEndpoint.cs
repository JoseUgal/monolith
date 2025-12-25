using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Tenants.Application.Tenants.GetMembers;
using Modules.Tenants.Application.Tenants.InviteMember;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Tenants.Endpoints.Tenants.InviteMember;

public sealed class InviteTenantMemberEndpoint(ISender sender) : Endpoint
{
    [HttpPost(TenantRoutes.InviteMember, Name = nameof(InviteTenantMemberEndpoint))]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Invite a new tenant member",
        Description = "Invites a new tenant member to the specified tenant.",
        Tags = [TenantRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid tenantId, [FromBody] InviteTenantMemberRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: CurrentUser
        var userId = Guid.Parse("7c2fd1d9-db1c-443a-a7e9-f2d106d6a04e");
        
        var command = new InviteTenantMemberCommand(
            tenantId,
            userId,
            request.UserId,
            request.Role
        );
        
        Result<Guid> result = await sender.Send(command, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) : Ok(result.Value);
    }
}
