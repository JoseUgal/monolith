using Domain.Results;
using Endpoints.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Users.Register;
using Modules.Users.Endpoints.Users.GetById;
using Swashbuckle.AspNetCore.Annotations;
using Endpoint = Endpoints.Endpoint;

namespace Modules.Users.Endpoints.Users.Register;

public sealed class RegisterUserEndpoint(ISender sender) : Endpoint
{
    [HttpPost(UserRoutes.Register)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerOperation(
        Summary = "Registers a new user.",
        Description = "Registers a new user based on the specified request.",
        Tags = [UserRoutes.Tag]
    )]
    public async Task<ActionResult<Guid>> HandleAsync(
        [FromBody] RegisterUserRequest request, 
        CancellationToken cancellationToken
    )
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password
        );
        
        Result<Guid> result = await sender.Send(command, cancellationToken);

        return result.IsFailure ? this.HandleFailure(result) : CreatedAtRoute(
            nameof(GetUserByIdEndpoint), 
            new { userId = result.Value }, 
            result.Value
        );
    }
}
