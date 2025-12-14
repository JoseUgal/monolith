namespace Modules.Users.Domain.Users;

/// <summary>
/// Represents the user repository interface.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds the specified user to the repository.
    /// </summary>
    /// <summary>
/// Adds the specified user to the repository.
/// </summary>
/// <param name="user">The user to add.</param>
    void Add(User user);
    
    /// <summary>
    /// Checks if the specified email is unique.
    /// </summary>
    /// <param name="email">The email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <summary>
/// Determines whether the specified email is not associated with any existing user.
/// </summary>
/// <param name="email">The email to check for uniqueness.</param>
/// <returns><c>true</c> if the email is not associated with any existing user, <c>false</c> otherwise.</returns>
    Task<bool> IsEmailUniqueAsync(UserEmail email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the user with the specified identifier, if it exists.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <summary>
/// Retrieves the user with the specified identifier.
/// </summary>
/// <returns>The user with the specified identifier, or null if no user exists with that identifier.</returns>
    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);
}