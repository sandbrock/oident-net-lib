using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

public static class ClientRedirectUriMapping
{
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientRedirectUri>(entity =>
        {
            entity.ToTable("client");

            entity.HasKey(e => e.ClientRedirectUriId);
            
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.ClientRedirectUriId).HasColumnName("client_redirect_uri_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Uri).HasColumnName("uri").HasMaxLength(2048);

            entity.HasIndex(e => e.ClientId);

            entity
                .HasOne<Client>()
                .WithMany()
                .HasForeignKey("client_id")
                .HasPrincipalKey(e => e.ClientId);
        });
    }
}