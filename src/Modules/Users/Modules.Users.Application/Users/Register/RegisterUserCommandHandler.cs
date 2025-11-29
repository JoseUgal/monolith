using Application.Messaging;
using Domain.Errors;
using Domain.Results;
using Modules.Users.Application.Abstractions.Security;
using Modules.Users.Domain.Users;

namespace Modules.Users.Application.Users.Register;

/// <summary>
/// Represents the <see cref="RegisterUserCommand"/> handler.
/// </summary>
internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher    
) : ICommandHandler<RegisterUserCommand, Guid>
{
    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (!UserFirstName.Create(command.FirstName).TryGetValue(out UserFirstName firstName, out Error error))
        {
            return Result.Failure<Guid>(error);
        }
        
        if (!UserLastName.Create(command.LastName).TryGetValue(out UserLastName lastName, out error))
        {
            return Result.Failure<Guid>(error);
        }
        
        if (!UserEmail.Create(command.Email).TryGetValue(out UserEmail email, out error))
        {
            return Result.Failure<Guid>(error);
        }
        
        if (!UserPassword.Create(command.Password).TryGetValue(out UserPassword password, out error))
        {
            return Result.Failure<Guid>(error);
        }

        var user = User.Create(firstName, lastName, email, passwordHasher.Hash(password.Value));
        
        throw new NotImplementedException("Register user is not implemented yet");
    }
}
