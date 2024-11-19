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
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? AuthorizationCode { get; set; }
    public string? State { get; set; }
    public string? Resource { get; set; }
    public string? Scope { get; set; }
}