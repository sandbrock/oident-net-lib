using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class ResourceServerScopeMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResourceServerScope>(entity =>
        {
            entity.ToTable("resource_server_scope");

            entity.HasKey(e => e.ResourceServerScopeId);

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.ResourceServerScopeId).HasColumnName("resource_server_scope_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.Property(e => e.ResourceServerId).HasColumnName("resource_server_id");

            entity.HasIndex(e => e.ResourceServerId);
            entity.HasIndex(e => new { ResourceServer = e.ResourceServerId, e.Name }).IsUnique();

            entity
                .HasOne<ResourceServer>()
                .WithMany()
                .HasForeignKey(e => e.ResourceServerId)
                .HasPrincipalKey(e => e.ResourceServerId);
        });
    }    
}