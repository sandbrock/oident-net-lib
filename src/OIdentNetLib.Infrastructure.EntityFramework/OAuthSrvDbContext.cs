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

    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientResourceServerScope> ClientAudienceScopes { get; set; }
    public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
    public DbSet<ResourceServer> ResourceServers { get; set; }
    public DbSet<ResourceServerScope> ResourceServerScopes { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ClientMapping.InitializeModelSchema(modelBuilder);
        ClientResourceServerScopeMapping.InitializeModelSchema(modelBuilder);
        ClientRedirectUriMapping.InitializeModelSchema(modelBuilder);
        ResourceServerMapping.InitializeModelSchema(modelBuilder);
        ResourceServerScopeMapping.InitializeModelSchema(modelBuilder);
        TenantMapping.InitializeModelSchema(modelBuilder);
        TenantEndpointMapping.InitializeModelSchema(modelBuilder);
        
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