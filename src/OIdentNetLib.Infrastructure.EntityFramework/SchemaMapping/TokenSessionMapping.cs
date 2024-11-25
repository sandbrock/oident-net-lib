using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class TokenSessionMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TokenSession>(entity =>
        {
            entity.ToTable("token_session");

            entity.HasKey(e => e.TokenSessionId);

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.TokenSessionId).HasColumnName("token_session_id");
            entity.Property(e => e.PreviousRefreshTokenId)
                .HasColumnName("previous_refresh_token_id")
                .IsRequired(false);
            entity.Property(e => e.PreviousRefreshTokenExpiresAt)
                .HasColumnName("previous_refresh_token_expires_at")
                .IsRequired(false);
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired(false);

            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => e.UserId);

            entity
                .HasOne<Client>()
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .HasPrincipalKey(e => e.ClientId);

            entity
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.UserId);
        });
    }
}