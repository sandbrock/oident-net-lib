using Microsoft.IdentityModel.Tokens;

namespace OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

public class MakeJwtRequest
{
    public JsonWebKey? Jwk { get; set; }
    public string? Issuer { get; set; }
    
    public string? Jti { get; set; }
    public string? Sub { get; set; }
    public string? Azp { get; set; }
    
    public string? SessionId { get; set; }
    public string? Audience { get; set; }
    public string? Scope { get; set; }
    
    public string? OriginalAudience { get; set; }
    public string? OriginalRole { get; set; }
    
    public DateTime? IssuedAt { get; set; }
    public DateTime? NotBefore { get; set; }
    public DateTime? Expires { get; set; }
}