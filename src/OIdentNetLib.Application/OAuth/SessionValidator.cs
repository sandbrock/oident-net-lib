using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;

namespace OIdentNetLib.Application.OAuth;

public class SessionValidator(
    ILogger<SessionValidator> logger,
    IAuthorizationSessionValidator authorizationSessionValidator,
    ITokenSessionValidator tokenSessionValidator) : ISessionValidator
{
    public async Task<GenericHttpResponse<ValidateSessionResponse>> ValidateAsync(ValidateSessionRequest validateSessionRequest)
    {
        if (!string.IsNullOrEmpty(validateSessionRequest.RefreshToken))
        {
            return await authorizationSessionValidator.ValidateAsync(validateSessionRequest);
        }

        if (!string.IsNullOrEmpty(validateSessionRequest.AuthorizationCode))
        {
            return await tokenSessionValidator.ValidateAsync(validateSessionRequest);
        }

        logger.LogInformation("Either refresh_token or authorization_code must be specified.");
        return GenericHttpResponse<ValidateSessionResponse>.CreateErrorResponse(
            HttpStatusCode.Unauthorized,
            OAuthErrorTypes.AccessDenied,
            "No session found for request.");
    }
}