namespace OIdentNetLib.Infrastructure.Database;

public class TokenSession : BaseModel
{
    public Guid? TokenSessionId { get; set; }
    public DateTime? SessionCreatedAt { get; set; }
    public DateTime? SessionExpiresAt { get; set; }
    public Guid? RefreshTokenId { get; set; }
    public Guid? PreviousRefreshTokenId { get; set; }
    public DateTime? PreviousRefreshTokenExpiresAt { get; set; }
    
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
}