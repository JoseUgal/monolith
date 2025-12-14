using Application.Messaging;
using Domain.Errors;
using Domain.Results;
using Modules.Users.Domain;
using Modules.Users.Domain.Users;

namespace Modules.Users.Application.Users.Update;

/// <summary>
/// Represents the <see cref="UpdateUserCommand"/> handler.
/// </summary>
internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateUserCommand>
{
    /// <inheritdoc />
    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = new UserId(command.UserId);
        
        User? user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(
                UserErrors.NotFound(userId)
            );
        }

        if (!UserFirstName.Create(command.FirstName).TryGetValue(out UserFirstName firstName, out Error error))
        {
            return Result.Failure(error);
        }
        
        if (!UserLastName.Create(command.LastName).TryGetValue(out UserLastName lastName, out error))
        {
            return Result.Failure(error);
        }
        
        user.Change(firstName, lastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
