using Microsoft.EntityFrameworkCore;

namespace OIdentNetLib.Infrastructure.EntityFramework.Entities;

public class Client : BaseEntity
{
    public Guid? ClientId { get; set; }
    
    public Guid? TenantId { get; set; }
    
    public Tenant? Tenant { get; set; }
    
    public string? Name { get; set; }
    
    public string? IconUrl { get; set; }
    
    public string? ClientSecretHash { get; set; }
    
    public IList<string>? GrantTypes { get; set; }
    
    public Uri? ClientUri { get; set; }
    
    public IList<ClientRedirectUri>? RedirectUris { get; set; }
    
    public IList<ClientAudienceScope>? AudienceScopes { get; set; }

    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            InitializeBaseEntity(entity);

            entity.ToTable("client");

            entity.HasKey(e => e.ClientId);
            
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(50);
            entity.Property(e => e.IconUrl)
                .HasColumnName("icon_url")
                .HasMaxLength(200);
            entity.Property(e => e.ClientSecretHash)
                .HasColumnName("client_secret_hash")
                .HasMaxLength(300);
            entity.Property(e => e.GrantTypes).HasColumnName("grant_types");
            entity.Property(e => e.ClientUri)
                .HasColumnName("client_uri")
                .HasMaxLength(200);

            entity.HasIndex(e => e.TenantId);
            entity.HasIndex(e => new { e.TenantId, e.Name }).IsUnique();

            entity
                .HasOne<Tenant>()
                .WithMany()
                .HasForeignKey("tenant_id")
                .HasPrincipalKey(e => e.TenantId);
        });
    }
}