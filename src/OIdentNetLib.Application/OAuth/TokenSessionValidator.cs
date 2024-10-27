using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Encryption.DataTransferObjects;
using OIdentNetLib.Infrastructure.Encryption.Models;

namespace OIdentNetLib.Application.OAuth;

public class TokenSessionValidator(
    ILogger<AuthorizationSessionValidator> logger,
    IClientReader clientReader,
    IUserReader userReader,
    IJwtValidator jwtValidator) : ITokenSessionValidator
{
    public async Task<GenericHttpResponse<ValidateSessionResponse>> ValidateAsync(ValidateSessionRequest validateSessionRequest)
    {
        var validateJwtRequest = new ValidateJwtRequest
        {
            Jwt = validateSessionRequest.RefreshToken,
        };
        
        var validateJwtResponse = await jwtValidator.ValidateAsync(validateJwtRequest);
        if (!validateJwtResponse.IsValid)
        {
            return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized, 
                OAuthErrorTypes.AccessDenied,
                "Invalid refresh token.");
        }

        if (validateJwtResponse.PrincipalType == JwtPrincipalType.User)
        {
            return await ReadUserSessionAsync(validateJwtResponse);
        }

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
                OAuthErrorTypes.AccessDenied,
                "Invalid refresh token.");
        }
        
        return GenericHttpResponse<ValidateSessionResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK, 
            new ValidateSessionResponse
            {
                SessionId = validateJwtResponse.SessionId,
                OAuthSessionType = OAuthSessionType.Token,
                PrincipalId = validateJwtResponse.PrincipalId,
                PrincipalName = client.Name,
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
                OAuthErrorTypes.AccessDenied,
                "Invalid refresh token.");
        }
        
        return GenericHttpResponse<ValidateSessionResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK, 
            new ValidateSessionResponse
            {
                SessionId = validateJwtResponse.SessionId,
                OAuthSessionType = OAuthSessionType.Token,
                PrincipalId = validateJwtResponse.PrincipalId,
                PrincipalName = user.Username,
                PrincipalEmail = user.Email,
                TenantId = user.TenantId
            }
        );
    }
}