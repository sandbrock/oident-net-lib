using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Validates the client session. It validates both authorization and token sessions.
/// </summary>
public class SessionValidator(
    ILogger<SessionValidator> logger,
    IAuthorizationSessionValidator authorizationSessionValidator,
    ITokenSessionValidator tokenSessionValidator
) : ISessionValidator
{
    public async Task<GenericHttpResponse<ValidateSessionResponse>> ValidateAsync(ValidateSessionRequest validateSessionRequest)
    {
        if (!string.IsNullOrEmpty(validateSessionRequest.RefreshToken))
        {
            return await tokenSessionValidator.ValidateAsync(validateSessionRequest);
        }

        if (!string.IsNullOrEmpty(validateSessionRequest.AuthorizationCode))
        {
            return await authorizationSessionValidator.ValidateAsync(validateSessionRequest);
        }

        logger.LogInformation("Either refresh_token or authorization_code must be specified.");
        return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
            HttpStatusCode.Unauthorized,
            OIdentErrors.InvalidSession,
            OAuthErrorTypes.AccessDenied,
            "No session found for request.");
    }
}