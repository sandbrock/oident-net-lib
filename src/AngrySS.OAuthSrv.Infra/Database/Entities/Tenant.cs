using Microsoft.EntityFrameworkCore;

namespace AngrySS.OAuthSrv.Infra.Database.Entities;

public class Tenant : BaseEntity
{
    public Guid TenantId { get; set; }
    
    public string? Name { get; set; }
    
    public static void InitializeModelSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            InitializeBaseEntity(entity);

            entity.ToTable("tenant");

            entity.HasKey(e => e.TenantId);

            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(50);

            entity.HasIndex(e => e.Name).IsUnique();
        });
    }
}