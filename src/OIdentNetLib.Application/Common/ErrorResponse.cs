using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.Common;

/// <summary>
/// Represents an OAuth error. The OAuth specification mentions the use of 
/// "error" and "error_description" properties in the response.
/// </summary>
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