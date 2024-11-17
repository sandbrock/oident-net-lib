using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Processes authorization_code OAuth flow
/// </summary>
public class AuthorizationCodeProcessor(
    IClientValidator clientValidator,
    IAuthorizationSessionValidator authorizationSessionValidator,
    IAuthorizationSessionWriter authorizationSessionWriter
) : IAuthorizationCodeProcessor
{
    public async Task<GenericHttpResponse<ProcessTokenResponse>> ProcessAsync(
        RequestMetadata requestMetadata,
        ProcessTokenRequest processTokenRequest)
    {
        // Validate the request object
        var validateRequestResult = ValidateRequestObject(processTokenRequest);
        if (!validateRequestResult.IsSuccess)
            return validateRequestResult;
        
        // Validate the response type
        if (processTokenRequest.GrantType != "authorization_code")
        {
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidResponseType,
                OAuthErrorTypes.InvalidRequest,
                "Invalid response_type");
        }
        
        // Parse the client_id
        if (!Guid.TryParse(processTokenRequest.ClientId, out var clientId))
        {
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidClientId,
                OAuthErrorTypes.InvalidRequest,
                "Invalid client_id");
        }
        
        // Validate the client
        var validateClientResponse = await clientValidator.ValidateAsync(new ValidateClientRequest
        {
            ClientId = clientId,
            ClientSecret = processTokenRequest.ClientSecret,
            RedirectUri = processTokenRequest.RedirectUri
        });
        if (!validateClientResponse.IsSuccess)
        {
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                validateClientResponse.StatusCode,
                validateClientResponse.OIdentError,
                validateClientResponse.Error,
                validateClientResponse.ErrorDescription);
        }
        
        // Validate the authorization session
        var validateSessionResponse = await authorizationSessionValidator.ValidateAsync(new ValidateSessionRequest
        {
            AuthorizationCode = processTokenRequest.Code
        });
        if (!validateSessionResponse.IsSuccess)
        {
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                validateSessionResponse.StatusCode,
                validateClientResponse.OIdentError,
                validateSessionResponse.Error,
                validateSessionResponse.ErrorDescription);
        }
        
        // Delete the authorization session
        await authorizationSessionWriter.DeleteAsync(validateSessionResponse.Data!.SessionId!.Value);
        
        // Generate the token
        
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
    
    private GenericHttpResponse<ProcessTokenResponse> ValidateRequestObject(ProcessTokenRequest request)
    {
        // Get object validation results
        var objectValidationResults = ObjectValidator.ValidateObject(request);

        if (objectValidationResults.IsSuccess)
            return GenericHttpResponse<ProcessTokenResponse>.CreateSuccessResponse(HttpStatusCode.OK);

        return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
            objectValidationResults.StatusCode,
            objectValidationResults.OIdentError,
            objectValidationResults.Error,
            objectValidationResults.ErrorDescription);
    }
}