using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Tenants.Application.Tenants.Create;
using Modules.Tenants.Endpoints.Tenants.GetById;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Tenants.Endpoints.Tenants.Create;

public sealed class CreateTenantEndpoint(ISender sender) : Endpoint
{
    [HttpPost(TenantRoutes.Create)]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerOperation(
        Summary = "Create a new tenant (organization/account)",
        Description = "Creates a new tenant (organization/account) owned by the currently authenticated user.",
        Tags = [TenantRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(CreateTenantRequest request, CancellationToken cancellation)
    {
        // TODO: Gets the user identifier from CurrentUser.
        var userId = Guid.Parse("7c2fd1d9-db1c-443a-a7e9-f2d106d6a04e");
        
        var command = new CreateTenantCommand(
            userId,
            request.Name,
            request.Slug
        );
        
        Result<Guid> result = await sender.Send(command, cancellation);

        return result.IsFailure ? this.HandleFailure(result) : CreatedAtRoute(
            nameof(GetTenantByIdEndpoint),
            new { tenantId = result.Value },
            result.Value
        );
    }
}
