using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using OIdentNetLib.Infrastructure.Encryption.Contracts;

namespace OIdentNetLib.Infrastructure.Encryption;

public class JwkCreator : IJwkCreator
{
    public JsonWebKey Create()
    {
        using var rsa = RSA.Create();
        var rsaSecurityKey = new RsaSecurityKey(rsa)
        {
            KeyId = Guid.NewGuid().ToString()
        };

        var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);
        return jwk;
    }
}