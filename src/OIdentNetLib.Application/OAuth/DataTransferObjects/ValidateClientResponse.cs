using OIdentNetLib.Infrastructure.Database;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ValidateClientResponse
{
    public Guid? ClientId { get; set; }
    public string? ClientName { get; set; }
    public ClientRedirectUri? ClientRedirectUri { get; set; }
}