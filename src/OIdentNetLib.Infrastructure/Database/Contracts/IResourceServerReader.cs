using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IResourceServerReader
{
    public Task<ResourceServer?> GetByIdAsync(Guid resourceServerId);
    public Task<ResourceServer?> GetByNameAsync(Guid tenantId, string name);
    public Task<PagedResponse<ResourceServer>> GetByTenantAsync(Guid tenantId, PagedRequest pagedRequest);
}