using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;
using OIdentNetLib.Infrastructure.Encryption.Models;

namespace OIdentNetLib.Infrastructure.Encryption;

public class JwtValidator() : IJwtValidator
{
    public async Task<ValidateJwtResponse> ValidateAsync(ValidateJwtRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.Jwk);
        ArgumentException.ThrowIfNullOrEmpty(request.Jwt);
        ArgumentException.ThrowIfNullOrEmpty(request.Issuer);
        ArgumentException.ThrowIfNullOrEmpty(request.Audience);

        var rsaParameters = new RSAParameters
        {
            Modulus = Base64UrlEncoder.DecodeBytes(request.Jwk.N),
            Exponent = Base64UrlEncoder.DecodeBytes(request.Jwk.E),
        };
        
        var securityKey = new RsaSecurityKey(rsaParameters) { KeyId = request.Jwk.Kid };

        var validationParameters = new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidIssuer = request.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateLifetime = true,
            ValidateActor = true,
        };

        var handler = new JwtSecurityTokenHandler();
        var validationResult = await handler.ValidateTokenAsync(request.Jwt, validationParameters);
        if (!validationResult.IsValid)
        {
            return new ValidateJwtResponse
            {
                IsValid = false
            };
        }

        var securityToken = validationResult.SecurityToken as JwtSecurityToken;
        ArgumentNullException.ThrowIfNull(securityToken);
            
        if (!Enum.TryParse(
            GetJwtClaimValue(securityToken, JwtClaimNames.PrincipalType),
            out JwtPrincipalType jwtPrincipalType))
        {
            return new ValidateJwtResponse { IsValid = false };
        }

        if (!Guid.TryParse(securityToken.Subject, out Guid principalId))
        {
            return new ValidateJwtResponse() { IsValid = false };
        }
        
        if (!Guid.TryParse(GetJwtClaimValue(securityToken, JwtClaimNames.SessionId), out Guid sessionId))
        {
            return new ValidateJwtResponse() { IsValid = false };
        }

        return new ValidateJwtResponse
        {
            IsValid = true,
            Audience = securityToken.Audiences.FirstOrDefault(),
            Issuer = securityToken.Issuer,
            KeyId = securityKey.KeyId,
            OriginalAudience = GetJwtClaimValue(securityToken, JwtClaimNames.OriginalAudience),
            OriginalScope = GetJwtClaimValue(securityToken, JwtClaimNames.OriginalScope),
            PrincipalId = principalId, 
            PrincipalType = jwtPrincipalType,
            Scope = GetJwtClaimValue(securityToken, JwtClaimNames.Scope),
            SessionId = sessionId
        };
    }

    private string? GetJwtClaimValue(JwtSecurityToken token, string claimType)
    {
        return token.Claims.FirstOrDefault(e => e.Type == claimType)?.Value;
    }
}