using Application.Messaging;
using Domain.Errors;
using Domain.Results;
using Modules.Users.Application.Abstractions.Security;
using Modules.Users.Domain;
using Modules.Users.Domain.Users;

namespace Modules.Users.Application.Users.Register;

/// <summary>
/// Represents the <see cref="RegisterUserCommand"/> handler.
/// </summary>
internal sealed class RegisterUserCommandHandler(
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IUnitOfWork  unitOfWork
) : ICommandHandler<RegisterUserCommand, Guid>
{
    /// <summary>
    /// Handles a RegisterUserCommand to validate input, create a new user, ensure the email is unique, persist the user, and produce the created user's identifier.
    /// </summary>
    /// <param name="command">Command containing the user's first name, last name, email, and password.</param>
    /// <param name="cancellationToken">Token to observe while awaiting repository and unit-of-work operations.</param>
    /// <returns>
    /// A successful Result containing the created user's <see cref="Guid"/> identifier when registration succeeds; a failure Result containing a domain validation error or an email-uniqueness error otherwise.
    /// </returns>
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

        if (!await userRepository.IsEmailUniqueAsync(email, cancellationToken))
        {
            return Result.Failure<Guid>(
                UserErrors.EmailIsNotUnique           
            );
        }
        
        userRepository.Add(user);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id.Value;
    }
}