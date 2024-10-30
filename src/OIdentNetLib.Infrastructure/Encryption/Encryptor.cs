using OIdentNetLib.Infrastructure.Encryption.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace OIdentNetLib.Infrastructure.Encryption;

public class Encryptor(ICryptoKeyReader cryptoKeyReader) : IEncryptor
{
    public const int KeyLength = 32;
    public const int IvLength = 16;
    
    public async Task<string> EncryptAsync(string plaintext)
    { 
        // Get the key
        var key = await cryptoKeyReader.ReadAsync();
        if (key.Length != KeyLength)
            throw new ArgumentException("Key length must be 256 bits (32 bytes) for AES-256.");

        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        await using var memStream = new MemoryStream();
        await memStream.WriteAsync(aes.IV, 0, aes.IV.Length);
        await using var cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
        await cryptoStream.WriteAsync(plainBytes, 0, plainBytes.Length);
        await cryptoStream.FlushAsync();
        await cryptoStream.FlushFinalBlockAsync();
        var cipherBytes = memStream.ToArray();
        return Convert.ToBase64String(cipherBytes);
    }
}