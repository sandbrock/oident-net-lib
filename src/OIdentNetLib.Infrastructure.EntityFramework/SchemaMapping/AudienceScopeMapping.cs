using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class AudienceScopeMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AudienceScope>(entity =>
        {
            entity.ToTable("audience_scope");

            entity.HasKey(e => e.AudienceScopeId);

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.AudienceScopeId).HasColumnName("audience_scope_id");
            entity.Property(e => e.AudienceId).HasColumnName("audience_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);

            entity.HasIndex(e => e.AudienceId);
            entity.HasIndex(e => new { e.AudienceId, e.Name }).IsUnique();

            entity
                .HasOne<Audience>()
                .WithMany()
                .HasForeignKey("audience_id")
                .HasPrincipalKey(e => e.AudienceId);
        });
    }    
}