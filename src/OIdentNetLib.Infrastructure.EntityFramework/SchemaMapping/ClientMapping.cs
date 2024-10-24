using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class ClientMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("client");

            entity.HasKey(e => e.ClientId);

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(2048)
                .IsRequired(false);
            entity.Property(e => e.ClientSecretHash).HasColumnName("client_secret_hash").HasMaxLength(128);
            entity.Property(e => e.GrantTypes).HasColumnName("grant_types");
            entity.Property(e => e.IconUrl)
                .HasColumnName("icon_url")
                .HasMaxLength(2048)
                .IsRequired(false);
            entity.Property(e => e.PrivacyUri)
                .HasColumnName("privacy_uri")
                .HasMaxLength(2048)
                .IsRequired(false);

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