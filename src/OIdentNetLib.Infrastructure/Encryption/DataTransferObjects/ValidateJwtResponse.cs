using System.IdentityModel.Tokens.Jwt;
using OIdentNetLib.Infrastructure.Encryption.Models;

namespace OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

public class ValidateJwtResponse
{
    public bool IsValid { get; set; }

    public string? Audience { get; set; }
    public string? Issuer { get; set; }
    public string? KeyId { get; set; }
    public string? OriginalAudience { get; set; }
    public string? OriginalScope { get; set; }
    public Guid? PrincipalId { get; set; }
    public JwtPrincipalType PrincipalType { get; set; }
    public string? Scope { get; set; }
    public Guid? SessionId { get; set; }
}