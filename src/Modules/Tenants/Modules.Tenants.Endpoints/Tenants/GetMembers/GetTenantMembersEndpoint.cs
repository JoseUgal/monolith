using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Tenants.Application.Tenants.GetMembers;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Tenants.Endpoints.Tenants.GetMembers;

public sealed class GetTenantMembersEndpoint(ISender sender) : Endpoint
{
    [HttpGet(TenantRoutes.GetMembers, Name = nameof(GetTenantMembersEndpoint))]
    [ProducesResponseType(typeof(TenantMemberResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get tenant members",
        Description = "Returns members for the given tenant.",
        Tags = [TenantRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var query = new GetTenantMembersQuery(tenantId);
        
        Result<TenantMemberResponse[]> result = await sender.Send(query, cancellationToken);
        
        return result.IsFailure ? this.HandleFailure(result) : Ok(result.Value);
    }
}
