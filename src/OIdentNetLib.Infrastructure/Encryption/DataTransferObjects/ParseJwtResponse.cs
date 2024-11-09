using Microsoft.IdentityModel.JsonWebTokens;

namespace OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

public class ParseJwtResponse
{
    public ParseJwtResponse(JsonWebToken jsonWebToken)
    {
        JsonWebToken = jsonWebToken;
    }
    
    public JsonWebToken JsonWebToken { get; }
    public string? TokenId { get; set; }
    public string? KeyId { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Subject { get; set; }
    public string? AuthorizedParty { get; set; }
    public string? Scope { get; set; }
    public DateTime? IssuedAt { get; set; }
    public DateTime? NotBefore { get; set; }
    public DateTime? Expires { get; set; }
}