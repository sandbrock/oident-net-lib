namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ProcessAuthoriziationCodeRequest
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public Uri? RedirectUri { get; set; }
    public string? Code { get; set; }
}