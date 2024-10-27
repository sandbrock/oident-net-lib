using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.Options;

public class OIdentOptions
{
    [JsonPropertyName("LoginUri")]
    public Uri? LoginUri { get; set; }
    
    [JsonPropertyName("AuthorizationSessionExpirationInMinutes")]
    public int AuthorizationSessionExpirationInMinutes { get; set; }
    
    [JsonPropertyName("TokenSessionExpirationInMinutes")]
    public int TokenSessionExpirationInMinutes { get; set; }
}