using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface ITenantReader
{
    Task<PagedResponse<Tenant>> GetAllAsync(PagedRequest pagedRequest);
    Task<Tenant?> GetByHostAsync(string host);
    Task<Tenant?> GetByPathAsync(string path);
}