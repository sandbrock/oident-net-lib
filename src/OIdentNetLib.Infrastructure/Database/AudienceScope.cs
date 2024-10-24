namespace OIdentNetLib.Infrastructure.Database;

public class AudienceScope : BaseModel
{
    public Guid? AudienceScopeId { get; set; }
    public string? Name { get; set; }
    
    public Guid? AudienceId { get; set; }
    public Audience? Audience { get; set; }
    
    public IList<ClientAudienceScope>? ClientAudienceScopes { get; set; }
}