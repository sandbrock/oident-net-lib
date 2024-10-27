namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IAuthorizationSessionReader
{
    Task<AuthorizationSession?> ReadByAuthCodeAsync(string authorizationCode);
}