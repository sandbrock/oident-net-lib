using System.IdentityModel.Tokens.Jwt;
using OIdentNetLib.Infrastructure.Encryption.Contracts;

namespace OIdentNetLib.Infrastructure.Encryption;

public class JwtDeserializer : IJwtDeserializer
{
    public JwtSecurityToken Deserialize(string tokenString)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);
        ArgumentNullException.ThrowIfNull(token);
        return token;
    }
}