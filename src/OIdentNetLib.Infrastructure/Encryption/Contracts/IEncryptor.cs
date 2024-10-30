namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IEncryptor
{
    Task<string> EncryptAsync(string plaintext);
}