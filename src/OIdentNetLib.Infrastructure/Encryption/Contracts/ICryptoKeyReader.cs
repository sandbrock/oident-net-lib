namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface ICryptoKeyReader
{
    Task<byte[]> ReadAsync();
}