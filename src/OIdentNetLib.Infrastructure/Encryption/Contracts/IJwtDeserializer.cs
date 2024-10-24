using System.IdentityModel.Tokens.Jwt;

namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IJwtDeserializer
{
    JwtSecurityToken Deserialize(string tokenString);
}