using System.Net;
using Microsoft.Extensions.Options;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.Options;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

public class TokenSessionCreator(
    IOptions<OIdentOptions> options,
    IJwtCreator jwtCreator,
    ITokenSessionWriter tokenSessionWriter
) : ITokenSessionCreator
{
    public async Task<GenericHttpResponse<CreateTokenSessionResponse>> CreateAsync(CreateTokenSessionRequest createTokenSessionRequest)
    {
        var issuedAt = DateTime.UtcNow;
        var accessTokenId = Guid.NewGuid();
        var refreshTokenId = Guid.NewGuid();
        var createTokenSessionResponse = new CreateTokenSessionResponse
        {
            AccessToken = CreateToken(createTokenSessionRequest, accessTokenId, issuedAt).Jwt,
            RefreshToken = CreateToken(createTokenSessionRequest, refreshTokenId, issuedAt, true).Jwt
        };

        var tokenSession = new TokenSession()
        {
            TokenSessionId = createTokenSessionRequest.SessionId,
            SessionCreatedAt = issuedAt,
            SessionExpiresAt = issuedAt.AddSeconds(options.Value.AccessTokenExpirationInSeconds),
            RefreshTokenId = refreshTokenId,
            ClientId = createTokenSessionRequest.ClientId,
            UserId = createTokenSessionRequest.UserId
        };
        await tokenSessionWriter.WriteAsync(tokenSession);
        
        return GenericHttpResponse<CreateTokenSessionResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK,
            createTokenSessionResponse);
    }

    private CreateJwtResponse CreateToken(
        CreateTokenSessionRequest createTokenSessionRequest,
        Guid tokenId,
        DateTime issuedAt,
        bool isRefreshToken = false)
    {
        var createJwtRequest = new CreateJwtRequest()
        {
            Issuer = options.Value.JwtIssuer,
            Jti = tokenId.ToString(),
            Sub = createTokenSessionRequest.Subject,
            SessionId = createTokenSessionRequest.SessionId.ToString(),
            PrincipalType = createTokenSessionRequest.PrincipalType,
            Audience = createTokenSessionRequest.Audience,
            Scope = createTokenSessionRequest.Scope,
            IssuedAt = issuedAt,
            NotBefore = issuedAt,
            Expires = issuedAt.AddMinutes(options.Value.AccessTokenExpirationInSeconds)
        };

        if (isRefreshToken)
        {
            createJwtRequest.Audience = options.Value.JwtIssuer;
            createJwtRequest.Expires = issuedAt.AddMinutes(options.Value.RefreshTokenExpirationInSeconds);
            createJwtRequest.OriginalAudience = createTokenSessionRequest.Audience;
            createJwtRequest.OriginalScope = createTokenSessionRequest.Scope;
        }
        
        var createJwtResponse = jwtCreator.Create(createJwtRequest);
        return createJwtResponse;
    }
}