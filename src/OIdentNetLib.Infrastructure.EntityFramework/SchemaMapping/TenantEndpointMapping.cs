using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class TenantEndpointMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TenantEndpoint>(entity =>
        {
            entity.ToTable("tenant_endpoint");
            
            entity.HasKey(e => e.TenantEndpointId);
            
            entity.Property(e => e.TenantEndpointId).HasColumnName("tenant_endpoint_id");
            entity.Property(e => e.Host)
                .HasColumnName("host")
                .HasMaxLength(50);
            entity.Property(e => e.Path)
                .HasColumnName("path")
                .HasMaxLength(50);
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => e.Host);
            entity.HasIndex(e => e.Path);
            
            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId)
                .HasPrincipalKey(e => e.TenantId);
        });
    }
}