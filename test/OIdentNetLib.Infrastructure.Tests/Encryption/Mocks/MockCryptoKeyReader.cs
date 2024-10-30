using System.Security.Cryptography;
using Moq;
using OIdentNetLib.Infrastructure.Encryption.Contracts;

namespace OIdentNetLib.Infrastructure.Tests.Encryption.Mocks;

public static class MockCryptoKeyReader
{
    private static readonly byte[] Key;
    
    static MockCryptoKeyReader()
    {
        Key = new byte[32];
        RandomNumberGenerator.Fill(Key);        
    }
    
    public static ICryptoKeyReader Get()
    {
        var mock = new Mock<ICryptoKeyReader>();
        mock.Setup(x => x.ReadAsync())
            .ReturnsAsync(() => Key);
        return mock.Object;
    }
}