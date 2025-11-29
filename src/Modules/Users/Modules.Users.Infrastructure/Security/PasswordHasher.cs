using System.Security.Cryptography;
using Application.ServiceLifetimes;
using Modules.Users.Application.Abstractions.Security;

namespace Modules.Users.Infrastructure.Security;

/// <summary>
/// Provides password hashing and verification using PBKDF2 with SHA-512.
/// </summary>
public class PasswordHasher : IPasswordHasher, ISingleton
{
    private const int SaltSize = 16;
    
    private const int HashSize = 32;
    
    private const int Iterations = 100000;
    
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    /// <inheritdoc />
    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    /// <inheritdoc />
    public bool Verify(string password, string passwordHash)
    {
        string[] parts = passwordHash.Split('-');
        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
        
        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
