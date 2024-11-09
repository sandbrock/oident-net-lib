using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.Options;

/// <summary>
/// A list of options used by the OIdentNetLib framework.
/// </summary>
public class OIdentOptions
{
    [JsonPropertyName("LoginUri")]
    public Uri? LoginUri { get; set; }
    
    [JsonPropertyName("AuthorizationSessionExpirationInMinutes")]
    public int AuthorizationSessionExpirationInMinutes { get; set; }
    
    [JsonPropertyName("TokenSessionExpirationInMinutes")]
    public int TokenSessionExpirationInMinutes { get; set; }
    
    [JsonPropertyName("JwtIssuer")]
    public string? JwtIssuer { get; set; }
}