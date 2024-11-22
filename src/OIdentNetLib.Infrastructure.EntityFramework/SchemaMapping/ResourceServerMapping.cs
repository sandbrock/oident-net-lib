using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class ResourceServerMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResourceServer>(entity =>
        {
            entity.HasKey(e => e.ResourceServerId);

            entity.ToTable("resource_server");

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ResourceServerId).HasColumnName("resource_server_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(2048)
                .IsRequired(false);
            entity.Property(e => e.LogoUrl)
                .HasColumnName("logo_url")
                .HasMaxLength(1024)
                .IsRequired(false);
            entity.Property(e => e.WebsiteUrl)
                .HasColumnName("website_url")
                .HasMaxLength(1024)
                .IsRequired(false);
            entity.Property(e => e.PrivacyPolicyUrl)
                .HasColumnName("privacy_policy_url")
                .HasMaxLength(1024)
                .IsRequired(false);
            entity.Property(e => e.TermsOfServiceUrl)
                .HasColumnName("terms_of_service_url")
                .HasMaxLength(1024)
                .IsRequired(false);
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => new { e.TenantId, e.Name }).IsUnique();

            entity
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .HasPrincipalKey(e => e.TenantId);
        });
    }
}