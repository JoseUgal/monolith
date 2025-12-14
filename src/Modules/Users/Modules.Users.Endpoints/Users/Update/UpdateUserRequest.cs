using Microsoft.AspNetCore.Mvc;

namespace Modules.Users.Endpoints.Users.Update;

/// <summary>
/// Represents the request for updating a user's basic information.
/// </summary>
public sealed class UpdateUserRequest
{
    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    [FromRoute(Name = UserRoutes.ResourceId)]
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets the content.
    /// </summary>
    [FromBody]
    public UpdateUserRequestContent Content { get; init; } = new(string.Empty, string.Empty);
}
