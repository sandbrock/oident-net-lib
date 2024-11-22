namespace OIdentNetLib.Infrastructure.Database;

public class ResourceServerScope : BaseModel
{
    public Guid? ResourceServerScopeId { get; set; }
    public string? Name { get; set; }
    
    public Guid? ResourceServerId { get; set; }
    public ResourceServer? ResourceServer { get; set; }
}