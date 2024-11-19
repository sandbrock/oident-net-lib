using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.Options;

/// <summary>
/// A list of options used by the OIdentNetLib framework.
/// </summary>
public class OIdentOptions
{
    [JsonPropertyName("JwtIssuer")]
    public string? JwtIssuer { get; set; }

    [JsonPropertyName("LoginUri")]
    public Uri? LoginUri { get; set; }
    
    [JsonPropertyName("AuthorizationSessionExpirationInSeconds")]
    public int AuthorizationSessionExpirationInSeconds { get; set; }
    
    [JsonPropertyName("TokenSessionExpirationInSeconds")]
    public int TokenSessionExpirationInSeconds { get; set; }
    
    [JsonPropertyName("AccessTokenExpirationInSeconds")]
    public int AccessTokenExpirationInSeconds { get; set; }

    [JsonPropertyName("RefreshTokenExpirationInSeconds")]
    public int RefreshTokenExpirationInSeconds { get; set; }
}