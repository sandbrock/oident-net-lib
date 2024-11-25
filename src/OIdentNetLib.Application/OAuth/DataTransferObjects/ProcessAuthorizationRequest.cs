using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebUtilities;

namespace OIdentNetLib.Application.OAuth.DataTransferObjects;

public class ProcessAuthorizationRequest : IParsable<ProcessAuthorizationRequest>
{
    [JsonPropertyName("response_type")]
    [Required(ErrorMessage = "response_type is required.")]
    public string? ResponseType { get; set; }

    [JsonPropertyName("client_id")]
    [Required(ErrorMessage = "client_id is required.")]
    public string? ClientId { get; set; }
    
    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }
    
    [JsonPropertyName("redirect_uri")]
    [Required(ErrorMessage = "redirect_uri is required.")]
    public string? RedirectUri { get; set; }
    
    [JsonPropertyName("scope")]
    public string? Scope { get; set; }
    
    [JsonPropertyName("resource")]
    public string? Resource { get; set; }
    
    [JsonPropertyName("state")]
    public string? State { get; set; }
    
    [JsonPropertyName("code_challenge")]
    [Required(ErrorMessage = "code_challenge is required.")]
    public string? CodeChallenge { get; set; }
    
    [JsonPropertyName("code_challenge_method")]
    [Required(ErrorMessage = "code_challenge_method is required.")]
    public string? CodeChallengeMethod { get; set; }

    public static ProcessAuthorizationRequest Parse(string s, IFormatProvider? provider)
    {
        var query = QueryHelpers.ParseQuery(s);
        return new ProcessAuthorizationRequest
        {
            ClientId = query["client_id"],
            ResponseType = query["response_type"],
            ClientSecret = query["client_secret"],
            RedirectUri = query["redirect_uri"],
            Resource = query["resource"],
            Scope = query["scope"],
            State = query["state"],
            CodeChallenge = query["code_challenge"],
            CodeChallengeMethod = query["code_challenge_method"]
        };
    }

    public static bool TryParse(
        [NotNullWhen(true)] string? s, 
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out ProcessAuthorizationRequest result)
    {
        try
        {
            result = Parse(s!, provider);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }
}