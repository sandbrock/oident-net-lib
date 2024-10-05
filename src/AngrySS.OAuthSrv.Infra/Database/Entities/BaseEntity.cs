using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AngrySS.OAuthSrv.Infra.Database.Entities;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }

    protected static void InitializeBaseEntity<T>(EntityTypeBuilder<T> entity) where T : BaseEntity
    {
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        entity.HasIndex(e => e.CreatedAt);
        entity.HasIndex(e => e.UpdatedAt);
    }
}