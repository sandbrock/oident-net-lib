using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Models;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Validates the user has a valid authorization session. 
/// This session is created when the user successfully fulfills 
/// the requirements of the authorization endpoint.
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
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidAuthorizationCode,
                OAuthErrorTypes.InvalidRequest,
                "Invalid authorization code.");
        }
        
        if (authSession.SessionExpiresAt < DateTime.UtcNow)
        {
            await authorizationSessionWriter.DeleteAsync(authSession.AuthorizationSessionId!.Value);
            logger.LogInformation(
                "Authorization session {AuthorizationSessionId} has expired.",
                authSession.AuthorizationSessionId);
            return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.ExpiredAuthorizationCode,
                OAuthErrorTypes.InvalidRequest,
                "Authorization session has expired.");
        }

        if (authSession.User != null)
        {
            return GenericHttpResponse<ValidateSessionResponse>.CreateSuccessResponseWithData(
                HttpStatusCode.OK,
                new ValidateSessionResponse
                {
                    SessionId = authSession.AuthorizationSessionId,
                    OAuthSessionType = OAuthSessionType.Authorization,
                    PrincipalType = JwtPrincipalType.User,
                    TenantId = authSession.User.TenantId,
                    ClientId = authSession.ClientId,
                    ClientRedirectUriId = authSession.ClientRedirectUriId,
                    UserId = authSession.UserId,
                    UserName = authSession.User.Username,
                    Email = authSession.User.Email,
                    AuthorizationCode = authSession.AuthorizationCode,
                    State = authSession.State,
                    Resource = authSession.Resource,
                    Scope = authSession.Scope
                }
            );
        }

        logger.LogInformation(
            "Authorization session {AuthorizationSessionId} has no user.",
            authSession.AuthorizationSessionId);
        await authorizationSessionWriter.DeleteAsync(authSession.AuthorizationSessionId!.Value);
        return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
            HttpStatusCode.BadRequest, 
            OIdentErrors.InvalidAuthorizationCode,
            OAuthErrorTypes.InvalidRequest,
            "Invalid authorization code.");
    }
}