using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Users.GetById;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Users.Endpoints.Users.GetById;

public sealed class GetUserByIdEndpoint(ISender sender) : Endpoint
{
    [HttpGet(UserRoutes.GetById)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get user by identifier.",
        Description = "Retrieves the details of a user identified by the specified unique identifier.",
        Tags = [UserRoutes.Tag]
    )]
    public async Task<ActionResult<UserResponse>> HandleAsync(
        Guid userId,
        CancellationToken cancellationToken
    )
    {
        var query = new GetUserByIdQuery(userId);
        
        Result<UserResponse> result = await sender.Send(query, cancellationToken);

        return result.IsFailure ? this.HandleFailure(result) : Ok(result.Value);
    }
}
