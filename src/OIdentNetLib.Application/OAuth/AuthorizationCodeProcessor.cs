using System.Net;
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
    ITenantValidator tenantValidator,
    IAuthorizationSessionValidator authorizationSessionValidator,
    IAuthorizationSessionWriter authorizationSessionWriter,
    ITokenSessionCreator tokenSessionCreator
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
        
        // Validate the grant type
        if (processTokenRequest.GrantType != "authorization_code")
        {
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidResponseType,
                OAuthErrorTypes.InvalidRequest,
                "Invalid response_type");
        }
        
        // Validate the client
        var validateClientResponse = await clientValidator.ValidateAsync(new ValidateClientRequest
        {
            ClientId = processTokenRequest.ClientId,
            ClientSecret = processTokenRequest.ClientSecret,
            RedirectUri = processTokenRequest.RedirectUri,
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
        
        // Generate the token response
        var createTokenSessionRequest = new CreateTokenSessionRequest()
        {
            SessionId = validateSessionResponse.Data!.SessionId,
            ClientId = validateClientResponse.Data!.ClientId,
            UserId = validateSessionResponse.Data!.UserId,
            Subject = validateSessionResponse.Data.UserId.ToString(),
            PrincipalType = validateSessionResponse.Data.PrincipalType,
            Audience = validateSessionResponse.Data.Resource,
            Scope = validateSessionResponse.Data.Scope,
        };
        var createTokenSessionResponse = await tokenSessionCreator.CreateAsync(createTokenSessionRequest);
        if (!createTokenSessionResponse.IsSuccess)
        {
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                createTokenSessionResponse.StatusCode,
                createTokenSessionResponse.OIdentError,
                createTokenSessionResponse.Error,
                createTokenSessionResponse.ErrorDescription);
        }

        // Delete the authorization session
        await authorizationSessionWriter.DeleteAsync(validateSessionResponse.Data!.SessionId!.Value);

        var processTokenResponse = new ProcessTokenResponse()
        {
            AccessToken = createTokenSessionResponse.Data!.AccessToken,
            TokenType = "Bearer",
            ExpiresIn = createTokenSessionResponse.Data!.ExpiresIn,
            RefreshToken = createTokenSessionResponse.Data!.RefreshToken,
            Scope = createTokenSessionResponse.Data!.Scope
        };
        return GenericHttpResponse<ProcessTokenResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK,
            processTokenResponse);
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