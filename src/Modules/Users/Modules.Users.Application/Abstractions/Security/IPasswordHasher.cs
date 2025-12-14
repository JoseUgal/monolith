namespace Modules.Users.Application.Abstractions.Security;

/// <summary>
/// Provides functionality for hashing and verifying passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Generates a secure hashed representation of the specified plain-text password.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <summary>
/// Creates a secure hashed representation of the specified plain-text password.
/// </summary>
/// <param name="password">The plain-text password to hash.</param>
/// <returns>A hashed string that includes all data required to verify the password.</returns>
    string Hash(string password);
    
    /// <summary>
    /// Verifies whether the specified plain-text password matches the previously hashed value.
    /// </summary>
    /// <param name="password">The plain-text password provided by the user.</param>
    /// <param name="passwordHash">The stored hashed password value.</param>
    /// <summary>
/// Determines whether a plain-text password matches a stored hashed password.
/// </summary>
/// <param name="password">The plain-text password to verify.</param>
/// <param name="passwordHash">The stored hashed password produced by <c>Hash</c>, containing the salt and metadata required for verification.</param>
/// <returns><c>true</c> if the password is valid; otherwise, <c>false</c>.</returns>
    bool Verify(string password, string passwordHash);
}