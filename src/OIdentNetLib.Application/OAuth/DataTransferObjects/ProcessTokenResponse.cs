using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ProcessTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
    
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }
    
    [JsonPropertyName("expires_in")]
    public DateTime? ExpiresIn { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
}