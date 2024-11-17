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
using OIdentNetLib.Infrastructure.Errors;

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
        RequestMetadata requestMetadata,
        ProcessAuthorizationRequest processAuthorizationRequest,
        ValidateSessionRequest validateSessionRequest)
    {
        // Validate the client
        var validateClientRequest = new ValidateClientRequest
        {
            ClientId = processAuthorizationRequest.ClientId,
            RedirectUri = processAuthorizationRequest.RedirectUri
        };
        var validateClientResponse = await clientValidator.ValidateAsync(validateClientRequest);
        
        switch (validateClientResponse.OIdentError)
        {
            case OIdentErrors.InvalidClientId:
                return GenericHttpResponse<ProcessAuthorizationResponse>.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    validateClientResponse.OIdentError,
                    validateClientResponse.Error,
                    validateClientResponse.ErrorDescription);
            case OIdentErrors.InvalidClientSecret:
                return GenericHttpResponse<ProcessAuthorizationResponse>.CreateRedirectResponse(
                    processAuthorizationRequest.RedirectUri!,
                    OIdentErrors.InvalidClientSecret,
                    validateClientResponse.Error,
                    validateClientResponse.ErrorDescription);
        }

        if (!validateClientResponse.IsSuccess)
        {
            return GenericHttpResponse<ProcessAuthorizationResponse>.CreateErrorResponse(
                validateClientResponse.StatusCode,
                validateClientResponse.OIdentError,
                validateClientResponse.Error,
                validateClientResponse.ErrorDescription);
        }
        
        // Validate the request object
        var validateRequestResult = ValidateRequestObject(processAuthorizationRequest);
        if (!validateRequestResult.IsSuccess)
            return validateRequestResult;
        
        // Validate the response type
        if (processAuthorizationRequest.ResponseType != "code")
        {
            return GenericHttpResponse<ProcessAuthorizationResponse>.CreateRedirectResponse(
                processAuthorizationRequest.RedirectUri!,
                OIdentErrors.InvalidResponseType,
                OAuthErrorTypes.UnsupportedResponseType,
                "Unsupported response_type parameter.");
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
                new Uri(redirectUrl),
                OIdentErrors.ValidSessionFound,
                null,
                null);
        }
        
        // Redirect with authorization code if session is valid
        var authorizationCode = validateSessionResponse.Data!.AuthorizationCode;
        redirectUrl = $"{processAuthorizationRequest.RedirectUri}?code={authorizationCode}";
        if (!string.IsNullOrEmpty(validateSessionResponse.Data!.State))
        {
            redirectUrl += $"&state={validateSessionResponse.Data.State}";
        }

        return GenericHttpResponse<ProcessAuthorizationResponse>.CreateRedirectResponse(
            new Uri(redirectUrl),
            OIdentErrors.ValidSessionFound,
            null,
            null);
    }
    
    private GenericHttpResponse<ProcessAuthorizationResponse> ValidateRequestObject(ProcessAuthorizationRequest request)
    {
        // Get object validation results
        var objectValidationResults = ObjectValidator.ValidateObject(
            request, 
            ObjectValidator.ObjectValidatorResultType.SingleLine);

        if (objectValidationResults.IsSuccess)
            return GenericHttpResponse<ProcessAuthorizationResponse>.CreateSuccessResponse(HttpStatusCode.OK);

        return GenericHttpResponse<ProcessAuthorizationResponse>.CreateRedirectResponse(
            request.RedirectUri!,
            objectValidationResults.OIdentError,
            objectValidationResults.Error,
            objectValidationResults.ErrorDescription);
    }
}