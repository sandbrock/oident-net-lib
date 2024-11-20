using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Processes the client_credentials OAuth flow.
/// </summary>
public class ClientCredentialsProcessor(
    ILogger<ClientCredentialsProcessor> logger,
    IClientValidator clientValidator
) : IClientCredentialsProcessor
{
    public async Task<GenericHttpResponse<ProcessTokenResponse>> ProcessAsync(
        RequestMetadata requestMetadata,
        ProcessTokenRequest request)
    {
        // Validate the request object
        var validateRequestResult = ObjectValidator.ValidateObject(
            request,
            ObjectValidator.ObjectValidatorResultType.SingleLine);
        
        if (!validateRequestResult.IsSuccess)
        {
            logger.LogWarning("Invalid client_credentials request object: {ErrorDescription}", 
                validateRequestResult.ErrorDescription);
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidRequest,
                OAuthErrorTypes.InvalidRequest,
                validateRequestResult.ErrorDescription);
        }
        
        // Validate the client
        var validateClientRequest = new ValidateClientRequest()
        {
            ClientId = request.ClientId,
            ClientSecret = request.ClientSecret
        };
        var clientValidationResult = await clientValidator.ValidateAsync(
            validateClientRequest);
        if (!clientValidationResult.IsSuccess)
        {
            logger.LogWarning("Invalid client_credentials request object: {ErrorDescription}",
                clientValidationResult.ErrorDescription);
            return GenericHttpResponse<ProcessTokenResponse>.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                clientValidationResult.OIdentError,
                clientValidationResult.Error,
                clientValidationResult.ErrorDescription);
        }
        
        

        await Task.CompletedTask;
        throw new NotImplementedException();
    }
    
}