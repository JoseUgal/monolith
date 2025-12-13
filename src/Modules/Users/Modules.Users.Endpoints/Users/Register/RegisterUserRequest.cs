namespace Modules.Users.Endpoints.Users.Register;

/// <summary>
/// Represents the request for registering a new user.
/// </summary>
/// <param name="Email">The email.</param>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="Password">The plain password.</param>
public sealed record RegisterUserRequest(
    string FirstName, 
    string LastName, 
    string Email, 
    string Password
);
