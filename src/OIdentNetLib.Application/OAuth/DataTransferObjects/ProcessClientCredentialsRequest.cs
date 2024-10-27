using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ProcessClientCredentialsRequest
{   
    [JsonPropertyName("grant_type")]
    public string? GrantType { get; set; }
    
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }
    
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }
    
    [JsonPropertyName("resource")]
    public Uri? Resource { get; set; }
}