using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ProcessAuthorizationRequest
{
    [JsonPropertyName("response_type")]
    [Required(ErrorMessage = "response_type is required.")]
    public string? ResponseType { get; set; }

    [JsonPropertyName("client_id")]
    [Required(ErrorMessage = "client_id is required.")]
    public Guid? ClientId { get; set; }
    
    [JsonPropertyName("redirect_uri")]
    [Required(ErrorMessage = "redirect_uri is required.")]
    public Uri? RedirectUri { get; set; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
    
    [JsonPropertyName("state")]
    public string? State { get; set; }
    
    [JsonPropertyName("code_challenge")]
    public string? CodeChallenge { get; set; }
    
    [JsonPropertyName("code_challenge_method")]
    public string? CodeChallengeMethod { get; set; }
}