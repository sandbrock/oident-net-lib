using System.IdentityModel.Tokens.Jwt;

namespace OIdentNetLib.Infrastructure.Encryption.Contracts;

public interface IJwtParser
{
    JwtSecurityToken Parse(string token);
}