namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public bool VerifyPassword(string hashedPassword, string password);
}