using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Encryption.Models;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ValidateSessionResponse
{
    public Guid? SessionId { get; set; }
    public OAuthSessionType OAuthSessionType { get; set; }
    public JwtPrincipalType PrincipalType { get; set; }
    public Guid? TenantId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? ClientRedirectUriId { get; set; }
    public Guid? PrincipalId { get; set; }
    public string? PrincipalName { get; set; }
    public string? PrincipalEmail { get; set; }
    public string? AuthorizationCode { get; set; }
}