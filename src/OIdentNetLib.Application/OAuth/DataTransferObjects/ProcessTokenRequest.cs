using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ProcessTokenRequest
{
    [JsonPropertyName("grant_type")]
    [Required(ErrorMessage = "grant_type is required.")]
    public string? GrantType { get; set; }
    
    [JsonPropertyName("client_id")]
    [Required(ErrorMessage = "client_id is required.")]
    public string? ClientId { get; set; }
    
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }
    
    [JsonPropertyName("redirect_uri")]
    public Uri? RedirectUri { get; set; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
    
    [JsonPropertyName("resource")]
    public string? Resource { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }
}