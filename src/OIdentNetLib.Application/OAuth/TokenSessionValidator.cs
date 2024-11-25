using System.Net;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;
using OIdentNetLib.Infrastructure.Encryption.Models;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Validates the user has a valid token session. The lifespan of a token 
/// session is managed by the first refresh_token issued.
/// </summary>
public class TokenSessionValidator(
    ILogger<AuthorizationSessionValidator> logger,
    IClientReader clientReader,
    IUserReader userReader,
    IJwtValidator jwtValidator
) : ITokenSessionValidator
{
    public async Task<GenericHttpResponse<ValidateSessionResponse>> ValidateAsync(ValidateSessionRequest validateSessionRequest)
    {
        // Validate the JWT
        var validateJwtRequest = new ValidateJwtRequest
        {
            Jwt = validateSessionRequest.RefreshToken,
        };
        var validateJwtResponse = await jwtValidator.ValidateAsync(validateJwtRequest);
        if (!validateJwtResponse.IsValid)
        {
            return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized, 
                OIdentErrors.InvalidRefreshToken,
                OAuthErrorTypes.AccessDenied,
                "Invalid refresh token.");
        }

        // Read a user session
        if (validateJwtResponse.PrincipalType == JwtPrincipalType.User)
        {
            return await ReadUserSessionAsync(validateJwtResponse);
        }

        // Read a client session
        return await ReadClientSessionAsync(validateJwtResponse);
    }

    private async Task<GenericHttpResponse<ValidateSessionResponse>> ReadClientSessionAsync(
        ValidateJwtResponse validateJwtResponse)
    {
        var client = await clientReader.GetByIdAsync(validateJwtResponse.PrincipalId!.Value);
        if (client is null)
        {
            logger.LogInformation("Unable to find client with id {ClientId}", validateJwtResponse.PrincipalId);
            return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized, 
                OIdentErrors.InvalidRefreshToken,
                OAuthErrorTypes.AccessDenied,
                "Invalid refresh token.");
        }
        
        return GenericHttpResponse<ValidateSessionResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK, 
            new ValidateSessionResponse
            {
                SessionId = validateJwtResponse.SessionId,
                OAuthSessionType = OAuthSessionType.Token,
                UserId = validateJwtResponse.PrincipalId,
                UserName = client.Name,
                TenantId = client.TenantId
            }
        );
    }

    private async Task<GenericHttpResponse<ValidateSessionResponse>> ReadUserSessionAsync(ValidateJwtResponse validateJwtResponse)
    {
        var user = await userReader.ReadById(validateJwtResponse.PrincipalId!.Value);
        if (user is null)
        {
            logger.LogInformation($"Unable to find user with ID {validateJwtResponse.PrincipalId}.");
            return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized, 
                OIdentErrors.InvalidRefreshToken,
                OAuthErrorTypes.AccessDenied,
                "Invalid refresh token.");
        }
        
        return GenericHttpResponse<ValidateSessionResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK, 
            new ValidateSessionResponse
            {
                SessionId = validateJwtResponse.SessionId,
                OAuthSessionType = OAuthSessionType.Token,
                UserId = validateJwtResponse.PrincipalId,
                UserName = user.Username,
                Email = user.Email,
                TenantId = user.TenantId
            }
        );
    }
}