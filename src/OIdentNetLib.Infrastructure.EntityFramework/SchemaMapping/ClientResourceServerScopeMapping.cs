using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class ClientResourceServerScopeMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientResourceServerScope>(entity =>
        {
            entity.ToTable("client_resource_server_scope");

            entity.HasKey(e => new { e.ClientId, AudienceScopeId = e.ResourceServerScopeId });
            
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.ResourceServerScopeId).HasColumnName("resource_server_scope_id");

            entity
                .HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .HasPrincipalKey(e => e.ClientId);

            entity
                .HasOne(e => e.ResourceServerScope)
                .WithMany()
                .HasForeignKey(e => e.ResourceServerScopeId)
                .HasPrincipalKey(e => e.ResourceServerScopeId);
        });
    }
}