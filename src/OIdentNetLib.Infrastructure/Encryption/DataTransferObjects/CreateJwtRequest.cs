using Microsoft.IdentityModel.Tokens;
using OIdentNetLib.Infrastructure.Encryption.Models;

namespace OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

public class CreateJwtRequest
{
    public const string SubjectTypeUser = "user";
    public const string SubjectTypeClient = "client";
    
    public JsonWebKey? Jwk { get; set; }
    public string? Issuer { get; set; }
    
    public string? Jti { get; set; }
    public string? Sub { get; set; }
    
    public string? SessionId { get; set; }
    
    public JwtPrincipalType? PrincipalType { get; set; }
    public string? Audience { get; set; }
    public string? Scope { get; set; }
    
    public string? OriginalAudience { get; set; }
    public string? OriginalScope { get; set; }
    
    public DateTime? IssuedAt { get; set; }
    public DateTime? NotBefore { get; set; }
    public DateTime? Expires { get; set; }
}