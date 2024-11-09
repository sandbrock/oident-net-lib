namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IJwkWriter
{
    Task WriteAsync(Jwk jwk);
}