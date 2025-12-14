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

    /// <inheritdoc />
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
