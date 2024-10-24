namespace OIdentNetLib.Infrastructure.Database;

public class Client : BaseModel
{
    public Guid? ClientId { get; set; }
    
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ClientSecretHash { get; set; }
    public string? GrantTypes { get; set; }
    public string? IconUrl { get; set; }
    public string? PrivacyUri { get; set; }
    
    public IList<ClientRedirectUri>? RedirectUris { get; set; }
    public IList<ClientAudienceScope>? ClientAudienceScopes { get; set; }
    
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    
    public Dictionary<string, string>? CustomProperties { get; set; }
    public object? CustomObject { get; set; }
}