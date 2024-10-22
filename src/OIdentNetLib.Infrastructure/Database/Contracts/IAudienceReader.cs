using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IAudienceReader
{
    public Task<Audience?> GetByIdAsync(Guid audienceId);
    public Task<PagedResponse<Audience>> GetByTenantAsync(Guid tenantId, PagedRequest pagedRequest);
}