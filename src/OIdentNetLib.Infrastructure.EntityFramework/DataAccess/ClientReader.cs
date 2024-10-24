using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Database.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.EntityFramework.DataAccess;

public class ClientReader : IClientReader
{
    public Task<Client> GetAsync(Guid clientId)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<Client>> GetByTenantAsync(Guid tenantId, PagedRequest request)
    {
        throw new NotImplementedException();
    }
}