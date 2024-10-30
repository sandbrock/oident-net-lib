namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface ITokenSessionWriter
{
    Task DeleteAsync(Guid sessionId);
    Task WriteAsync(TokenSession tokenSession);
}