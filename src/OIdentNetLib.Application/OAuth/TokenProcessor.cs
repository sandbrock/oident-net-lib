using System.Net;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

public class TokenProcessor(
    IAuthorizationCodeProcessor authorizationCodeProcessor,
    IClientCredentialsProcessor clientCredentialsProcessor,
    IRefreshTokenProcessor refreshTokenProcessor
) : ITokenProcessor
{
    public async Task<GenericHttpResponse<ProcessTokenResponse>> ProcessAsync(
        RequestMetadata requestMetadata,
        ProcessTokenRequest processTokenRequest)
    {
        var validateObjectResult = ObjectValidator.ValidateObject(processTokenRequest);
        if (!validateObjectResult.IsSuccess)
        {
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                validateObjectResult.StatusCode,
                validateObjectResult.OIdentError,
                validateObjectResult.Error,
                validateObjectResult.ErrorDescription);
        }
        
        switch (processTokenRequest.GrantType)
        {
            case "authorization_code":
                return await authorizationCodeProcessor.ProcessAsync(requestMetadata, processTokenRequest);
            case "client_credentials":
                return await clientCredentialsProcessor.ProcessAsync(requestMetadata, processTokenRequest);
            case "refresh_token":
                return await refreshTokenProcessor.ProcessAsync(requestMetadata, processTokenRequest);
            default:
                return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    OIdentErrors.InvalidGrantType,
                    OAuthErrorTypes.InvalidRequest,
                    "Unsupported grant type");
        }
    }
}