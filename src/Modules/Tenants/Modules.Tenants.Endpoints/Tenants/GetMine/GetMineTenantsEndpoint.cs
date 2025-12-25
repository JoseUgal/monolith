using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Tenants.Application.Tenants.GetForUser;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Tenants.Endpoints.Tenants.GetMine;

public sealed class GetMineTenantsEndpoint(ISender sender) : Endpoint
{
    [HttpGet(TenantRoutes.GetMine, Name = nameof(GetMineTenantsEndpoint))]
    [ProducesResponseType(typeof(TenantResponse[]), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get mine tenants.",
        Description = "Get the tenants for the current user.",
        Tags = [TenantRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        // TODO: CurrentUser
        var userId = Guid.Parse("7c2fd1d9-db1c-443a-a7e9-f2d106d6a04e");

        var query = new GetTenantsForUserQuery(userId);
        
        Result<TenantResponse[]> result = await sender.Send(query, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) : Ok(result.Value);
    }
}
