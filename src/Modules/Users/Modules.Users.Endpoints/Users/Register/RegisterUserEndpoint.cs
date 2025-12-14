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
    /// <summary>
    /// Registers a new user using the information in the request.
    /// </summary>
    /// <param name="request">Registration details including first name, last name, email, and password.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The created user's Guid if registration succeeds; otherwise an <see cref="ActionResult"/> representing the failure.</returns>
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
            request.FirstName,
            request.LastName,
            request.Email,
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