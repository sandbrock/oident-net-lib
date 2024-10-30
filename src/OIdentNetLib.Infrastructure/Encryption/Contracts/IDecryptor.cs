namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IDecryptor
{
    Task<string> DecryptAsync(string encryptedText);
}