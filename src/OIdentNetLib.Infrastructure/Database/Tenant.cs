namespace OIdentNetLib.Infrastructure.Database;

public class Tenant
{
    public Guid TenantId { get; set; }
    public string? Name { get; set; }
    public string? IconUrl { get; set; }

    public Dictionary<string, string>? CustomProperties { get; set; }
    public object? CustomObject { get; set; }
}