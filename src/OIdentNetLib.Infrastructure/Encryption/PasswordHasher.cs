using System.Security.Cryptography;
using OIdentNetLib.Infrastructure.Encryption.Contracts;

namespace OIdentNetLib.Infrastructure.Encryption;

public class PasswordHasher : IPasswordHasher
{
    // Define parameters for PBKDF2
    private const int SaltSize = 16; // 128-bit salt
    private const int HashSize = 32; // 256-bit hash (SHA-256)
    private const int Iterations = 100000; // Number of iterations (should be high enough for security)
    
    public string HashPassword(string password)
    {
        // Generate a random salt
        var salt = new byte[SaltSize];
        RandomNumberGenerator.Fill(salt);

        // Hash the password using PBKDF2 with HMACSHA256
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(HashSize);

        // Combine the salt and the hash
        var hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convert the combined salt+hash to a base64 string
        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        // Extract the bytes from the base64 encoded hash
        var hashBytes = Convert.FromBase64String(hashedPassword);

        // Extract the salt from the stored hash
        var salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Extract the stored hash
        var storedHash = new byte[HashSize];
        Array.Copy(hashBytes, SaltSize, storedHash, 0, HashSize);

        // Hash the incoming password using the extracted salt
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var computedHash = pbkdf2.GetBytes(HashSize);

        // Compare the stored hash with the computed hash
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}