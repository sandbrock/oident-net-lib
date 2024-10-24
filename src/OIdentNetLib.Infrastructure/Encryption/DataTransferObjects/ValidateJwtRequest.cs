using Microsoft.IdentityModel.Tokens;

namespace OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

public class ValidateJwtRequest
{
    public JsonWebKey? Jwk { get; set; }
    public string? Jwt { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}