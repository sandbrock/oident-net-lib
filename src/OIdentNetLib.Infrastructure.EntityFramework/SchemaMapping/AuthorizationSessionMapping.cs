using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public class AuthorizationSessionMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorizationSession>(entity =>
        {
            entity.ToTable("authorization_session");

            entity.HasKey(e => e.AuthorizationSessionId);

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.AuthorizationSessionId).HasColumnName("authorization_session_id");
            entity.Property(e => e.ResponseType).HasColumnName("response_type").HasMaxLength(25);
            entity.Property(e => e.State).HasColumnName("state").HasMaxLength(1024);
            entity.Property(e => e.Scope)
                .HasColumnName("scope")
                .HasMaxLength(500)
                .IsRequired(false);
            entity.Property(e => e.CodeChallenge)
                .HasColumnName("code_challenge")
                .HasMaxLength(500);
            entity.Property(e => e.CodeChallengeMethod)
                .HasColumnName("code_challenge_method")
                .HasMaxLength(25);
            entity.Property(e => e.AuthorizationCode)
                .HasColumnName("authorization_code")
                .HasMaxLength(100);
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ClientRedirectUriId).HasColumnName("client_redirect_uri_id");

            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ClientRedirectUriId);

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

            entity
                .HasOne<ClientRedirectUri>()
                .WithMany()
                .HasForeignKey(e => e.ClientRedirectUriId)
                .HasPrincipalKey(e => e.ClientRedirectUriId);
        });
    }
}