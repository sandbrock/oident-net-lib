using OIdentNetLib.Infrastructure.Encryption.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace OIdentNetLib.Infrastructure.Encryption;

public class Decryptor(ICryptoKeyReader cryptoKeyReader) : IDecryptor
{
    public async Task<string> DecryptAsync(string encryptedText)
    {
        // Get the key
        var key = await cryptoKeyReader.ReadAsync();
        if (key.Length != Encryptor.KeyLength)
            throw new ArgumentException("Key length must be 256 bits (32 bytes) for AES-256.");
        
        // Create the AES object
        using var aes = Aes.Create();
        aes.Key = key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        
        // Get the encrypted bytes
        var cipherBytes = Convert.FromBase64String(encryptedText);
        
        // Get the IV
        byte[] iv = new byte[Encryptor.IvLength];
        Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
        aes.IV = iv;

        // Decrypt the ciphertext
        using var decryptor = aes.CreateDecryptor();
        await using var memStream = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length);
        await using var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);
        
        byte[] plainBytes = new byte[cipherBytes.Length - iv.Length];
        int bytesRead = await cryptoStream.ReadAsync(plainBytes, 0, plainBytes.Length);
        
        return Encoding.UTF8.GetString(plainBytes, 0, bytesRead);
    }
}