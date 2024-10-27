namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IAuthorizationSessionWriter
{
    Task DeleteAsync(Guid authorizationSessionId);
    Task WriteAsync(AuthorizationSession authorizationSession);
}