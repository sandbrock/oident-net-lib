using Microsoft.EntityFrameworkCore;

namespace OIdentNetLib.Infrastructure.EntityFramework.Entities;

public class ClientAudienceScope : BaseEntity
{
    public Guid? ClientId { get; set; }

    public Client? Client { get; set; }

    public Guid? AudienceScopeId { get; set; }
    
    public AudienceScope? AudienceScope { get; set; }
    
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientAudienceScope>(entity =>
        {
            InitializeBaseEntity(entity);
            
            entity.ToTable("client_audience_scope");

            entity.HasKey(e => new { e.ClientId, e.AudienceScopeId });
            
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.AudienceScopeId).HasColumnName("audience_scope_id");

            entity
                .HasOne(e => e.Client)
                .WithMany(e => e.AudienceScopes)
                .HasForeignKey(e => e.ClientId)
                .HasPrincipalKey(e => e.ClientId);

            entity
                .HasOne(e => e.AudienceScope)
                .WithMany(e => e.Clients)
                .HasForeignKey(e => e.AudienceScopeId)
                .HasPrincipalKey(e => e.AudienceScopeId);
        });
    }
}