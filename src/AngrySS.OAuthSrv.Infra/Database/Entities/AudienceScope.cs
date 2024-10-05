using Microsoft.EntityFrameworkCore;

namespace AngrySS.OAuthSrv.Infra.Database.Entities;

public class AudienceScope : BaseEntity
{
    public Guid? AudienceScopeId { get; set; }
    
    public Guid? AudienceId { get; set; }

    public Audience? Audience { get; set; }

    public string? Name { get; set; }
    
    public IList<ClientAudienceScope>? Clients { get; set; }

    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AudienceScope>(entity =>
        {
            InitializeBaseEntity(entity);
            
            entity.ToTable("audience_scope");

            entity.HasKey(e => e.AudienceScopeId);

            entity.Property(e => e.AudienceScopeId).HasColumnName("audience_scope_id");
            entity.Property(e => e.AudienceId).HasColumnName("audience_id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(50);

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