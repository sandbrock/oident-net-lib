namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ValidateClientRequest
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? RedirectUri { get; set; }
}