namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ValidateSessionRequest
{
    public string? AuthorizationCode { get; set; }
    public string? RefreshToken { get; set; }
}