namespace OIdentNetLib.Infrastructure.Database;

public class User : BaseModel
{
    public Guid? UserId { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    public Dictionary<string, string>? CustomProperties { get; set; }
    public object? CustomObject { get; set; }
}