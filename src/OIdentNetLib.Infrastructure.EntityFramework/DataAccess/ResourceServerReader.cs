using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.EntityFramework.DataAccess;

public class ResourceServerReader(OAuthSrvDbContext context) : IResourceServerReader
{
    public async Task<ResourceServer?> GetByIdAsync(Guid resourceServerId)
    {
        var query = context.ResourceServers
            .Where(a => a.ResourceServerId == resourceServerId);
        var queryResult = await query.FirstOrDefaultAsync();
        return queryResult;
    }

    public async Task<ResourceServer?> GetByNameAsync(Guid tenantId, string name)
    {
        var query = context.ResourceServers
            .Where(a => a.TenantId == tenantId && a.Name == name);
        var queryResult = await query.FirstOrDefaultAsync();
        return queryResult;
    }

    public async Task<PagedResponse<ResourceServer>> GetByTenantAsync(Guid tenantId, PagedRequest pagedRequest)
    {
        var totalQuery = context.ResourceServers
            .Where(t => t.TenantId == tenantId)
            .OrderBy(t => t.Name);
        
        var pagedQuery = totalQuery
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize);
        
        var entities = await pagedQuery.ToListAsync();

        var result = new PagedResponse<ResourceServer>()
        {
            Data = entities,
            PageNumber = pagedRequest.PageNumber,
            TotalRowCount = await totalQuery.CountAsync()
        };
        
        return result;
    }
}