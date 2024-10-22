using Microsoft.EntityFrameworkCore;

namespace OIdentNetLib.Infrastructure.EntityFramework.Entities;

public class Audience : BaseEntity
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
    
    public Database.Audience ToInfrastructureEntity()
    {
        return new Database.Audience()
        {
            AudienceId = AudienceId,
            Name = Name,
            Description = Description,
            LogoUrl = LogoUrl,
            WebsiteUrl = WebsiteUrl,
            PrivacyPolicyUrl = PrivacyPolicyUrl,
            TermsOfServiceUrl = TermsOfServiceUrl,
            TenantId = TenantId,
        };
    }

    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audience>(entity =>
        {
            InitializeBaseEntity(entity);

            entity.HasKey(e => e.AudienceId);

            entity.ToTable("audience");

            entity.Property(e => e.AudienceId).HasColumnName("audience_id");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(50);

            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => new { e.TenantId, e.Name }).IsUnique();

            entity
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey("tenant_id")
                .HasPrincipalKey(e => e.TenantId);
        });
    }
}