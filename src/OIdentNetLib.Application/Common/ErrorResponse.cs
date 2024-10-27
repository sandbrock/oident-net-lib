using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.Common;

public class ErrorResponse
{
    public ErrorResponse(string? error, string? errorDescription)
    {
        Error = error;
        ErrorDescription = errorDescription;
    }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
    
    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; set; }
}