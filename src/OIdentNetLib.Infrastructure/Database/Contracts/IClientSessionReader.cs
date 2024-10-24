namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IClientSessionReader
{
    Task<TokenSession> ReadAsync(Guid sessionId);
}