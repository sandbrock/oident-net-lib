namespace OIdentNetLib.Infrastructure.Database;

public class Client
{
    public Guid? ClientId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public IList<ClientRedirectUri>? RedirectUris { get; set; }
    
    public Dictionary<string, string>? CustomProperties { get; set; }
    public object? CustomObject { get; set; }
}