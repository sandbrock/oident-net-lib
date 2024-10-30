using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Models;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Validates the user has a valid authorization session. 
/// This session is created when the user successfully fulfills 
/// the requirements of the the authorization endpoint.
/// </summary>
public class AuthorizationSessionValidator(
    ILogger<AuthorizationSessionValidator> logger,
    IAuthorizationSessionReader authorizationSessionReader,
    IAuthorizationSessionWriter authorizationSessionWriter
) : IAuthorizationSessionValidator
{
    public async Task<GenericHttpResponse<ValidateSessionResponse>> ValidateAsync(ValidateSessionRequest validateSessionRequest)
    {
        var authSession = await authorizationSessionReader.ReadByAuthCodeAsync(validateSessionRequest.AuthorizationCode!);
        if (authSession is null)
        {
            logger.LogInformation(
                "Authorization session not found for code {AuthorizationCode}.", 
                validateSessionRequest.AuthorizationCode);
            return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized, 
                OAuthErrorTypes.AccessDenied,
                "Invalid authorization code.");
        }
        
        if (authSession.ExpiresAt < DateTime.UtcNow)
        {
            await authorizationSessionWriter.DeleteAsync(authSession.AuthorizationSessionId!.Value);
            logger.LogInformation(
                "Authorization session {AuthorizationSessionId} has expired.",
                authSession.AuthorizationSessionId);
            return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized, 
                OAuthErrorTypes.AccessDenied,
                "Authorization session has expired.");
        }

        if (authSession.User != null)
            return GenericHttpResponse<ValidateSessionResponse>.CreateSuccessResponseWithData(
                HttpStatusCode.OK,
                new ValidateSessionResponse
                {
                    SessionId = authSession.AuthorizationSessionId,
                    OAuthSessionType = OAuthSessionType.Authorization,
                    PrincipalType = JwtPrincipalType.User,
                    PrincipalId = authSession.UserId,
                    PrincipalName = authSession.User.Username,
                    PrincipalEmail = authSession.User.Email,
                    TenantId = authSession.User.TenantId
                }
            );
        
        logger.LogInformation(
            "Authorization session {AuthorizationSessionId} has no user.",
            authSession.AuthorizationSessionId);
        
        return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
            HttpStatusCode.Unauthorized, 
            OAuthErrorTypes.AccessDenied,
            "Invalid authorization code.");
    }
}