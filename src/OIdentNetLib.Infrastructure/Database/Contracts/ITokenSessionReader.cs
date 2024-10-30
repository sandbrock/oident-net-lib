namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface ITokenSessionReader
{
    Task<TokenSession?> ReadByIdAsync(Guid sessionId);
}