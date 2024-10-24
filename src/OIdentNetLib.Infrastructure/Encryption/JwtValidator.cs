using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

namespace OIdentNetLib.Infrastructure.Encryption;

public class JwtValidator : IJwtValidator
{
    public async Task<ValidateJwtResponse> Validate(ValidateJwtRequest request)
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
        
        return new ValidateJwtResponse
        {
            Jwt = validationResult.SecurityToken as JwtSecurityToken,
            IsValid = true
        };
    }
}