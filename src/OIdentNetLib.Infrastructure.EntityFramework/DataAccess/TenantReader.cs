using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.EntityFramework.DataAccess;

public class TenantReader(OAuthSrvDbContext context) : ITenantReader
{
    public async Task<PagedResponse<Tenant>> GetAllAsync(PagedRequest pagedRequest)
    {
        var totalQuery = context.Tenants
            .OrderBy(t => t.Name);
        
        var pagedQuery = totalQuery
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize);
        
        var entities = await pagedQuery.ToListAsync();

        var result = new PagedResponse<Tenant>()
        {
            Data = entities,
            PageNumber = pagedRequest.PageNumber,
            TotalRowCount = await totalQuery.CountAsync()
        };
        
        return result;
    }
}