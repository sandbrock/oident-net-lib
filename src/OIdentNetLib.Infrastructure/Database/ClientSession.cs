namespace OIdentNetLib.Infrastructure.Database;

public class ClientSession
{
    public Guid? ClientSessionId { get; set; }
    public DateTime? SessionCreatedAt { get; set; }
    public DateTime? SessionExpiresAt { get; set; }
    public Guid? RefreshTokenId { get; set; }
    public Guid? PreviousRefreshTokenId { get; set; }
    public DateTime? PreviousRefreshTokenExpiresAt { get; set; }
    
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    
    public object? CustomObject { get; set; }
}