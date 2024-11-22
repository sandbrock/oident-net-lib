namespace OIdentNetLib.Infrastructure.Database;

public class TenantEndpoint : BaseModel
{
    public Guid? TenantEndpointId { get; set; }

    public string? Host { get; set; }
    
    public string? Path { get; set; }
    
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}