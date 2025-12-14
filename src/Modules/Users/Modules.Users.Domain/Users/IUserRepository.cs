namespace Modules.Users.Domain.Users;

/// <summary>
/// Represents the user repository interface.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds the specified user to the repository.
    /// </summary>
    /// <param name="user">The user.</param>
    void Add(User user);
    
    /// <summary>
    /// Checks if the specified email is unique.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The success result if the email is unique, otherwise a failure result.</returns>
    Task<bool> IsEmailUniqueAsync(UserEmail email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the user with the specified identifier, if it exists.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user if exists, otherwise a nullable value.</returns>
    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
}
