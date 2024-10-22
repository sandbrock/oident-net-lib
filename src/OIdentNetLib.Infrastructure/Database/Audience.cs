namespace OIdentNetLib.Infrastructure.Database;

public class Audience
{
    public Guid? AudienceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? PrivacyPolicyUrl { get; set; }
    public string? TermsOfServiceUrl { get; set; }
    
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    
    public Dictionary<string, string>? CustomProperties { get; set; }
    public object? CustomObject { get; set; }
}