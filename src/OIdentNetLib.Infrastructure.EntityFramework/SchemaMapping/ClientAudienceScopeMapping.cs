using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class ClientAudienceScopeMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientAudienceScope>(entity =>
        {
            entity.ToTable("client_audience_scope");

            entity.HasKey(e => new { e.ClientId, e.AudienceScopeId });
            
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.AudienceScopeId).HasColumnName("audience_scope_id");

            entity
                .HasOne(e => e.Client)
                .WithMany(e => e.ClientAudienceScopes)
                .HasForeignKey(e => e.ClientId)
                .HasPrincipalKey(e => e.ClientId);

            entity
                .HasOne(e => e.AudienceScope)
                .WithMany(e => e.ClientAudienceScopes)
                .HasForeignKey(e => e.AudienceScopeId)
                .HasPrincipalKey(e => e.AudienceScopeId);
        });
    }
}