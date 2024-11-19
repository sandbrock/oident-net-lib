using OIdentNetLib.Infrastructure.Encryption.Models;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class CreateTokenSessionRequest
{
    public Guid? SessionId { get; set; }
    public Guid? ClientId { get; set; }
    public Guid? UserId { get; set; }
    public string? Subject { get; set; }
    public JwtPrincipalType? PrincipalType { get; set; }
    public string? Audience { get; set; }
    public string? Scope { get; set; }
}