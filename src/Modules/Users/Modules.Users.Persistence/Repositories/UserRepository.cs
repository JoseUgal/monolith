using Application.ServiceLifetimes;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Domain.Users;

namespace Modules.Users.Persistence.Repositories;

/// <summary>
/// Represents the user repository.
/// </summary>
/// <param name="dbContext">The database context.</param>
public sealed class UserRepository(UsersDbContext dbContext) : IUserRepository, IScoped
{
    /// <inheritdoc />
    public void Add(User user) => dbContext.Add(user);

    /// <summary>
    /// Checks whether the specified email is unique among all users.
    /// </summary>
    /// <param name="email">The email to check for uniqueness.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>`true` if no existing user has the specified email, `false` otherwise.</returns>
    public async Task<bool> IsEmailUniqueAsync(UserEmail email, CancellationToken cancellationToken = default)
    {
        return !await dbContext.Set<User>().AnyAsync(user =>
                user.Email == email,
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>().FirstOrDefaultAsync(user => 
            user.Id == userId, 
            cancellationToken
        );
    }
}