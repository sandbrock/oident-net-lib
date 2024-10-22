using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.EntityFramework.Entities;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Audience.InitializeModelSchema(modelBuilder);
        AudienceScope.InitializeModelSchema(modelBuilder);
        Client.InitializeModelSchema(modelBuilder);
        ClientAudienceScope.InitializeModelSchema(modelBuilder);
        ClientRedirectUri.InitializeModelSchema(modelBuilder);
        Tenant.InitializeModelSchema(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
    
    private void SetTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

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