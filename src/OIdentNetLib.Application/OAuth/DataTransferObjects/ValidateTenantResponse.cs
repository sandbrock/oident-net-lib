using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ValidateTenantResponse
{
    public Tenant? Tenant { get; set; }
}