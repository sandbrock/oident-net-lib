namespace OIdentNetLib.Infrastructure.Database;

public class ClientAudienceScope : BaseModel
{
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    
    public Guid? AudienceScopeId { get; set; }
    public AudienceScope? AudienceScope { get; set; }
    public List<AudienceScope>? AudienceScopes { get; set; }

}