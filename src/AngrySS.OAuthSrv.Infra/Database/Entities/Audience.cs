using Microsoft.EntityFrameworkCore;

namespace AngrySS.OAuthSrv.Infra.Database.Entities;

public class Audience : BaseEntity
{
    public Guid? AudienceId { get; set; }
    
    public Guid? TenantId { get; set; }

    public Tenant? Tenant { get; set; }

    public string? Name { get; set; }

    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audience>(entity =>
        {
            InitializeBaseEntity(entity);

            entity.HasKey(e => e.AudienceId);

            entity.ToTable("audience");

            entity.Property(e => e.AudienceId).HasColumnName("audience_id");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(50);

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