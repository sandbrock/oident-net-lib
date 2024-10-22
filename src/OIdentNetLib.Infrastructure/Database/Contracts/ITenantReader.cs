using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface ITenantReader
{
    Task<PagedResponse<Tenant>> GetAllAsync(PagedRequest pagedRequest);
}