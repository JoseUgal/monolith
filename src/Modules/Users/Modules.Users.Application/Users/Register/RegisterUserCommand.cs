using Application.Messaging;

namespace Modules.Users.Application.Users.Register;

/// <summary>
/// Represents the command for registering a new user.
/// </summary>
/// <param name="Email">The email.</param>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="Password">The plain password.</param>
public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password
) : ICommand<Guid>;
