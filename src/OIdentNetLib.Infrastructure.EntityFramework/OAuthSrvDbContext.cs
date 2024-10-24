using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.EntityFramework.SchemaMapping;

namespace OIdentNetLib.Infrastructure.EntityFramework;

public class OAuthSrvDbContext : DbContext
{
    public OAuthSrvDbContext(DbContextOptions<OAuthSrvDbContext> options): 
        base(options)
    {
    }

    public DbSet<Audience> Audiences { get; set; }
    public DbSet<AudienceScope> AudienceScopes { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientAudienceScope> ClientAudienceScopes { get; set; }
    public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AudienceMapping.InitializeModelSchema(modelBuilder);
        AudienceScopeMapping.InitializeModelSchema(modelBuilder);
        ClientMapping.InitializeModelSchema(modelBuilder);
        ClientAudienceScopeMapping.InitializeModelSchema(modelBuilder);
        ClientRedirectUriMapping.InitializeModelSchema(modelBuilder);
        TenantMapping.InitializeModelSchema(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void SetTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseModel>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }    
}