using Application.ServiceLifetimes;
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
}
