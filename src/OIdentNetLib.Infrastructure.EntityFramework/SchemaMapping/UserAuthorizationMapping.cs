using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class UserAuthorizationMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAuthorization>(entity =>
        {
            entity.ToTable("user_authorization");

            entity.HasKey(e => new { e.ClientId, e.UserId, e.AudienceScopeId });

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AudienceScopeId).HasColumnName("audience_scope_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");

            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.AudienceScopeId);
        });
    }
}