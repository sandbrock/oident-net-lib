using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Database.Contracts;

public interface IClientReader
{
    Task<Client?> GetByIdAsync(Guid clientId);
    Task<PagedResponse<Client>> GetByTenantAsync(Guid tenantId, PagedRequest request);
}