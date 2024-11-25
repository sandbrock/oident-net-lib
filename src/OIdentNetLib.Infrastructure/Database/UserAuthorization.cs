namespace OIdentNetLib.Infrastructure.Database;

public class UserAuthorization : BaseModel
{
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    
    public Guid? AudienceScopeId { get; set; }
    public ResourceServerScope? ResourceServerScope { get; set; }

    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
}