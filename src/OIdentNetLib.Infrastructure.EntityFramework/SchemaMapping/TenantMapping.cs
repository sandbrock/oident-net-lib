using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class TenantMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("tenant");

            entity.HasKey(e => e.TenantId);

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(2048)
                .IsRequired(false);
            entity.Property(e => e.IconUri)
                .HasColumnName("icon_uri")
                .HasMaxLength(2048)
                .IsRequired(false);
            entity.Property(e => e.PrivacyPolicyUri)
                .HasColumnName("privacy_policy_uri")
                .HasMaxLength(2048)
                .IsRequired(false);

            entity.HasIndex(e => e.Name).IsUnique();
        });
    }
}