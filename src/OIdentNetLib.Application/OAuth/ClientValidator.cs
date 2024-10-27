using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database.Contracts;

namespace OIdentNetLib.Application.OAuth;

public class ClientValidator(
    ILogger<ClientValidator> logger,
    IClientReader clientReader) : IClientValidator
{
    public async Task<GenericHttpResponse<ValidateClientResponse>> ValidateAsync(ValidateClientRequest validateClientRequest)
    {
        // Validate the client_id
        var client = await clientReader.GetByIdAsync(validateClientRequest.ClientId!.Value);
        if (client is null)
        {
            logger.LogInformation("Unable to locate client {ClientId}.", validateClientRequest.ClientId);
            return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OAuthErrorTypes.InvalidRequest,
                "Invalid client_id.");
        }
        
        // Validate the client's redirect_uris
        if (client.RedirectUris is null || client.RedirectUris.Count == 0)
        {
            logger.LogWarning("Client {ClientId} has no redirect_uris.", client.ClientId);
            return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OAuthErrorTypes.InvalidRequest,
                "Invalid redirect_uri.");
        }
        
        // Validate the requested redirect_uri
        var redirectUri = client.RedirectUris.FirstOrDefault(e => e.Uri == validateClientRequest.RedirectUri);
        if (redirectUri == null)
        {
            return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OAuthErrorTypes.InvalidRequest,
                "Invalid redirect_uri.");
        }

        // Create the response object
        var response = new ValidateClientResponse
        {
            ClientId = client.ClientId,
            ClientName = client.Name!,
            ClientRedirectUri = client.RedirectUris.FirstOrDefault(
                e => e.Uri == validateClientRequest.RedirectUri),
        };
        
        return GenericHttpResponse<ValidateClientResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK, 
            response);
    }
}