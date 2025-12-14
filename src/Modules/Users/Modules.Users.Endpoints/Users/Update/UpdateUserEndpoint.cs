using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Users.Update;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Users.Endpoints.Users.Update;

public sealed class UpdateUserEndpoint(ISender sender) : Endpoint
{
    [HttpPut(UserRoutes.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Updates the user's basic information.",
        Description = "Updates the user's basic information based on the specified request.",
        Tags = [UserRoutes.Tag]
    )]
    public async Task<ActionResult> HandleAsync(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(
            request.UserId,
            request.Content.FirstName,
            request.Content.LastName
        );
        
        Result result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? this.HandleFailure(result) : NoContent();
    }
}
