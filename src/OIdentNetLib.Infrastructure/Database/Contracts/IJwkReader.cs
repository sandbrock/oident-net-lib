namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IJwkReader
{
    Task<IList<Jwk>> ReadAllAsync();
}