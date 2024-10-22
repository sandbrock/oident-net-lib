using Microsoft.EntityFrameworkCore;

namespace OIdentNetLib.Infrastructure.EntityFramework.Entities;

public class Tenant : BaseEntity
{
    public Guid TenantId { get; set; }
    
    public string? Name { get; set; }
    
    public string? IconUrl { get; set; }
    
    public Database.Tenant ToInfrastructureEntity()
    {
        return new Database.Tenant
        {
            TenantId = TenantId,
            Name = Name,
            IconUrl = IconUrl
        };
    }
    
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
            entity.Property(e => e.IconUrl)
                .HasColumnName("icon_url")
                .HasMaxLength(200);

            entity.HasIndex(e => e.Name).IsUnique();
        });
    }
}