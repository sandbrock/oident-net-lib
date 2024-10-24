namespace OIdentNetLib.Infrastructure.Database;

public class AuthorizationSession : BaseModel
{
    public Guid? AuthorizationSessionId { get; set; }
    public string? ResponseType { get; set; }
    public string? State { get; set; }
    public string? Scope { get; set; }
    public string? CodeChallenge { get; set; }
    public string? CodeChallengeMethod { get; set; }
    
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    
    public Guid? ClientRedirectUriId { get; set; }
    public ClientRedirectUri? ClientRedirectUri { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
}