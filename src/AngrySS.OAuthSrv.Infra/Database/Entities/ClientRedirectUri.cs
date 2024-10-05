using Microsoft.EntityFrameworkCore;

namespace AngrySS.OAuthSrv.Infra.Database.Entities;

public class ClientRedirectUri : BaseEntity
{
    public Guid? ClientRedirectUriId { get; set; }
    
    public Guid? ClientId { get; set; }
    
    public Client? Client { get; set; }
    
    public Uri? Uri { get; set; }

    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientRedirectUri>(entity =>
        {
            InitializeBaseEntity(entity);

            entity.ToTable("client");

            entity.HasKey(e => e.ClientRedirectUriId);
            
            entity.Property(e => e.ClientRedirectUriId).HasColumnName("client_redirect_uri_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Uri).HasColumnName("uri");

            entity.HasIndex(e => e.ClientId);

            entity
                .HasOne<Client>()
                .WithMany()
                .HasForeignKey("client_id")
                .HasPrincipalKey(e => e.ClientId);
        });
    }
}