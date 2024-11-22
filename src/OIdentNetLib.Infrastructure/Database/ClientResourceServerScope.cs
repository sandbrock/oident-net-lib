namespace OIdentNetLib.Infrastructure.Database;

public class ClientResourceServerScope : BaseModel
{
    public Guid? ClientId { get; set; }
    public Client? Client { get; set; }
    
    public Guid? ResourceServerScopeId { get; set; }
    public ResourceServerScope? ResourceServerScope { get; set; }
}