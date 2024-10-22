using Microsoft.EntityFrameworkCore;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.EntityFramework.DataAccess;

public class AudienceReader(OAuthSrvDbContext context) : IAudienceReader
{
    public async Task<Audience?> GetByIdAsync(Guid audienceId)
    {
        var query = context.Audiences
            .Where(a => a.AudienceId == audienceId);
        var queryResult = await query.FirstOrDefaultAsync();
        
        if (queryResult == null)
        {
            return null;
        }
        
        return queryResult.ToInfrastructureEntity();
    }

    public async Task<PagedResponse<Audience>> GetByTenantAsync(Guid tenantId, PagedRequest pagedRequest)
    {
        var totalQuery = context.Audiences
            .Where(t => t.TenantId == tenantId)
            .OrderBy(t => t.Name);
        
        var pagedQuery = totalQuery
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize);
        
        var entities = await pagedQuery.ToListAsync();

        var result = new PagedResponse<Audience>()
        {
            Data = new List<Audience>(),
            PageNumber = pagedRequest.PageNumber,
            TotalRowCount = await totalQuery.CountAsync()
        };
        
        foreach (var entity in entities)
        {
            result.Data.Add(entity.ToInfrastructureEntity());
        }
        
        return result;
    }
}