using System.Net;
using Microsoft.Extensions.Options;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Application.Options;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Processes the authorization OAuth endpoint.
/// </summary>
public class AuthorizationProcessor(
    IOptions<OIdentOptions> oidentOptions,
    IClientValidator clientValidator,
    IAuthorizationCodeCreator authorizationCodeCreator,
    IAuthorizationSessionValidator authorizationSessionValidator,
    IAuthorizationSessionWriter authorizationSessionWriter
) : IAuthorizationProcessor
{
    public async Task<GenericHttpResponse<ProcessAuthorizationResponse>> ProcessAsync(
        ProcessAuthorizationRequest processAuthorizationRequest,
        ValidateSessionRequest validateSessionRequest)
    {
        // Validate the request object
        var validateRequestResult = ValidateRequestObject(processAuthorizationRequest);
        if (!validateRequestResult.IsSuccess)
            return validateRequestResult;
        
        // Validate the response type
        if (processAuthorizationRequest.ResponseType != "code")
        {
            return GenericHttpResponse<ProcessAuthorizationResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OAuthErrorTypes.InvalidRequest,
                "Invalid response_type");
        }
        
        // Validate the client
        var validateClientRequest = new ValidateClientRequest
        {
            ClientId = processAuthorizationRequest.ClientId,
            RedirectUri = processAuthorizationRequest.RedirectUri
        };
        
        var validateClientResponse = 
            await clientValidator.ValidateAsync(validateClientRequest);
        if (!validateClientResponse.IsSuccess)
        {
            return GenericHttpResponse<ProcessAuthorizationResponse>.CreateErrorResponse(
                validateClientResponse.StatusCode,
                validateClientResponse.Error,
                validateClientResponse.ErrorDescription);
        }

        // Check for existing session
        var validateSessionResponse = await authorizationSessionValidator.ValidateAsync(validateSessionRequest);
        string redirectUrl;
        if (!validateSessionResponse.IsSuccess)
        {
            redirectUrl = 
                $"{oidentOptions.Value.LoginUri}?" +
                $"response_type={processAuthorizationRequest.ResponseType}&" +
                $"client_id={processAuthorizationRequest.ClientId}&" +
                $"redirect_uri={processAuthorizationRequest.RedirectUri}&" +
                $"state={processAuthorizationRequest.State}&" +
                $"scope={processAuthorizationRequest.Scope}&" +
                $"code_challenge={processAuthorizationRequest.CodeChallenge}&" +
                $"code_challenge_method={processAuthorizationRequest.CodeChallengeMethod}";

            return GenericHttpResponse<ProcessAuthorizationResponse>.CreateRedirectResponse(
                new Uri(redirectUrl));
        }
        
        // Create a new session
        var authorizationCode = validateSessionResponse.Data!.AuthorizationCode;
        if (validateSessionResponse.Data!.OAuthSessionType == OAuthSessionType.Token)
        {
            var session = await CreateSessionAsync(processAuthorizationRequest, validateClientResponse.Data!);
            authorizationCode = session.AuthorizationCode;
        }
        
        // Redirect with authorization code if session is valid
        redirectUrl = $"{processAuthorizationRequest.RedirectUri}?code={authorizationCode}";
        if (!string.IsNullOrEmpty(processAuthorizationRequest.State))
            redirectUrl += "&{request.State}";

        return GenericHttpResponse<ProcessAuthorizationResponse>.CreateRedirectResponse(new Uri(redirectUrl));
    }

    private async Task<AuthorizationSession> CreateSessionAsync(
        ProcessAuthorizationRequest processAuthorizationRequest,
        ValidateClientResponse validateClientResponse)
    {
        var authorizationSessionId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var session = new AuthorizationSession
        {
            AuthorizationSessionId = authorizationSessionId,
            ResponseType = processAuthorizationRequest.ResponseType,
            State = processAuthorizationRequest.State,
            Scope = processAuthorizationRequest.Scope,
            CodeChallenge = processAuthorizationRequest.CodeChallenge,
            CodeChallengeMethod = processAuthorizationRequest.CodeChallengeMethod,
            AuthorizationCode = authorizationCodeCreator.Create(),
            ClientId = processAuthorizationRequest.ClientId,
            ClientRedirectUriId = validateClientResponse.ClientRedirectUri!.ClientRedirectUriId,
            ExpiresAt = now.AddMinutes(oidentOptions.Value.AuthorizationSessionExpirationInMinutes),
        };
        
        await authorizationSessionWriter.WriteAsync(session);

        return session;
    }
    
    private GenericHttpResponse<ProcessAuthorizationResponse> ValidateRequestObject(ProcessAuthorizationRequest request)
    {
        // Get object validation results
        var objectValidationResults = ObjectValidator.ValidateObject(request);

        if (objectValidationResults.IsSuccess)
            return GenericHttpResponse<ProcessAuthorizationResponse>.CreateSuccessResponse(HttpStatusCode.OK);

        return GenericHttpResponse<ProcessAuthorizationResponse>.CreateErrorResponse(
            objectValidationResults.StatusCode,
            objectValidationResults.Error,
            objectValidationResults.ErrorDescription);
    }
}