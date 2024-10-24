namespace OIdentNetLib.Infrastructure.Database;

public class Tenant : BaseModel
{
    public Guid TenantId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? IconUri { get; set; }
    public string? PrivacyPolicyUri { get; set; }

    public Dictionary<string, string>? CustomProperties { get; set; }
    public object? CustomObject { get; set; }
}