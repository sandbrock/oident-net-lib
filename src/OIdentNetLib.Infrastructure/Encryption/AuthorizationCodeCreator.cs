using System.Security.Cryptography;
using OIdentNetLib.Infrastructure.Encryption.Contracts;

namespace OIdentNetLib.Infrastructure.Encryption;

public class AuthorizationCodeCreator : IAuthorizationCodeCreator
{
    public string Create(int length = IAuthorizationCodeCreator.DefaultLength)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(length);
        var authorizationCode = Convert.ToBase64String(randomBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
        return authorizationCode;
    }
}