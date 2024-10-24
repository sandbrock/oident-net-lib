using System.IdentityModel.Tokens.Jwt;

namespace OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

public class ValidateJwtResponse
{
    public bool IsValid { get; set; }
    public JwtSecurityToken? Jwt { get; set; }
}