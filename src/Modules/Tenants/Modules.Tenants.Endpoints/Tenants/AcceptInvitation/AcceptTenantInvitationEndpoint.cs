using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Tenants.Application.Tenants.AcceptInvitation;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Tenants.Endpoints.Tenants.AcceptInvitation;


public sealed class AcceptTenantInvitationEndpoint(ISender sender) : Endpoint
{
    [HttpPost(TenantRoutes.AcceptInvitation, Name = nameof(AcceptTenantInvitationEndpoint))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerOperation(
        Summary = "Accept tenant invitation",
        Description = "Accepts an invitation to join a tenant.",
        Tags = [TenantRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid tenantId, Guid membershipId, CancellationToken cancellationToken = default)
    {
        // TODO: CurrentUser
        var userId = Guid.Parse("7c2fd1d9-db1c-443a-a7e9-f2d106d6a04e");
        
        var command = new AcceptTenantInvitationCommand(
            tenantId,
            membershipId,
            userId
        );
        
        Result result = await sender.Send(command, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) : NoContent();
    }
}
