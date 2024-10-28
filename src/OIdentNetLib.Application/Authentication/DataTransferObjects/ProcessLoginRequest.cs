namespace OIdentNetLib.Application.Authentication.DataTransferObjects;

public class ProcessLoginRequest
{
    public Guid? TenantId { get; set; }
    public Guid? ClientId { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}