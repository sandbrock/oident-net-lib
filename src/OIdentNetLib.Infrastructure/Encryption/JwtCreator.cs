using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace OIdentNetLib.Infrastructure.Encryption;

public class JwtCreator : IJwtCreator
{
    public MakeJwtResponse Create(MakeJwtRequest request)
    {
        ArgumentNullException.ThrowIfNull(request.Jwk);
        
        ArgumentException.ThrowIfNullOrEmpty(request.Jti);
        ArgumentException.ThrowIfNullOrEmpty(request.Sub);
        ArgumentException.ThrowIfNullOrEmpty(request.Azp);
        
        ArgumentException.ThrowIfNullOrEmpty(request.SessionId);
        ArgumentException.ThrowIfNullOrEmpty(request.Audience);
        ArgumentException.ThrowIfNullOrEmpty(request.Scope);
        
        ArgumentNullException.ThrowIfNull(request.IssuedAt);
        ArgumentNullException.ThrowIfNull(request.NotBefore);
        ArgumentNullException.ThrowIfNull(request.Expires);

        var rsaParameters = new RSAParameters()
        {
            Modulus = Base64UrlEncoder.DecodeBytes(request.Jwk.N),
            Exponent = Base64UrlEncoder.DecodeBytes(request.Jwk.E),
            D = Base64UrlEncoder.DecodeBytes(request.Jwk.D),
            DP = Base64UrlEncoder.DecodeBytes(request.Jwk.DP),
            DQ = Base64UrlEncoder.DecodeBytes(request.Jwk.DQ),
            P = Base64UrlEncoder.DecodeBytes(request.Jwk.P),
            Q = Base64UrlEncoder.DecodeBytes(request.Jwk.Q),
            InverseQ = Base64UrlEncoder.DecodeBytes(request.Jwk.QI)
        };
        
        var rsa = RSA.Create(rsaParameters);
        
        var key = new RsaSecurityKey(rsa);
        key.KeyId = request.Jwk.KeyId;
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
        
        var claims = new Dictionary<string, object>()
        {
            { JwtRegisteredClaimNames.Jti, request.Jti },
            { JwtRegisteredClaimNames.Sub, request.Sub },
            { JwtRegisteredClaimNames.Azp, request.Azp },
        };
        
        AddNonEmptyClaim(claims, "session_id", request.SessionId);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = request.Issuer,
            IssuedAt = request.IssuedAt,
            NotBefore = request.NotBefore,
            Expires = request.Expires,
            Audience = request.Audience,
            Claims = claims,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(descriptor);

        var response = new MakeJwtResponse
        {
            JwtId = request.Jti,
            Jwt = tokenHandler.WriteToken(token),
            Expires = request.Expires
        };

        return response;
    }

    private void AddNonEmptyClaim(Dictionary<string, object> claims, string key, string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }
        
        claims.Add(key, value);
    }
}