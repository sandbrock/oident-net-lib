namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ValidateClientRequest
{
    public Guid? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public Uri? RedirectUri { get; set; }
}