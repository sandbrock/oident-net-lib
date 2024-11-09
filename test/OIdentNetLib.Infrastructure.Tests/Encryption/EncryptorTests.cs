using OIdentNetLib.Infrastructure.Encryption;
using OIdentNetLib.Infrastructure.Tests.Encryption.Mocks;

namespace OIdentNetLib.Infrastructure.Tests.Encryption;

public class EncryptorTests
{
    [Fact]
    public async Task Encrypt_Decrypt_Return_Equals()
    {
        // Arrange
        var keyReader = MockCryptoKeyReader.Get();
        var encryptor = new Encryptor(keyReader);
        var decryptor = new Decryptor(keyReader);
        var text = "Hello World";

        // Act
        var encryptedText = await encryptor.EncryptAsync(text);
        var decryptedText = await decryptor.DecryptAsync(encryptedText);

        // Assert
        Assert.Equal(text, decryptedText);
    }
}