namespace OIdentNetLib.Infrastructure.Database;

public class UserAuthorizationToken : BaseModel
{
    public Guid? UserAuthorizationTokenId { get; set; }
    public string? Scope { get; set; }
    public string? Token { get; set; }
    public string? TokenType { get; set; }
    public DateTime? ExpiresAt { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    
    public Guid? AudienceId { get; set; }
    public Audience? Audience { get; set; }
}