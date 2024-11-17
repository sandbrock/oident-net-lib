using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Encryption.Contracts;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Validates and authenticates the client.
/// </summary>
public class ClientValidator(
    ILogger<ClientValidator> logger,
    IClientReader clientReader,
    IPasswordHasher passwordHasher
) : IClientValidator
{
    public async Task<GenericHttpResponse<ValidateClientResponse>> ValidateAsync(ValidateClientRequest validateClientRequest)
    {
        // Validate the value of client_id
        var client = await clientReader.GetByIdAsync(validateClientRequest.ClientId!.Value);
        if (client is null)
        {
            logger.LogInformation("Unable to locate client {ClientId}.", validateClientRequest.ClientId);
            return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidClientId,
                OAuthErrorTypes.InvalidClient,
                "Invalid client_id parameter.");
        }
        
        // Check if client_secret is required
        if (client.IsSecureClient && string.IsNullOrEmpty(validateClientRequest.ClientSecret))
        {
            logger.LogInformation("Client {ClientId} requires a client_secret.", validateClientRequest.ClientId);
            return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidClientSecret,
                OAuthErrorTypes.InvalidRequest,
                "The client_secret parameter is required for the specified client.");
        }

        // Validate the value of client_secret
        if (client.IsSecureClient)
        {
            var isSecretValid = passwordHasher.VerifyPassword(
                client.ClientSecretHash,
                validateClientRequest.ClientSecret);
            if (!isSecretValid)
            {
                logger.LogInformation("Request for {ClientId} has an invalid client_secret parameter.",
                    validateClientRequest.ClientId);
                return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    OIdentErrors.InvalidClientSecret,
                    OAuthErrorTypes.UnauthorizedClient,
                    "Invalid client_secret parameter.");
            }
        }
        
        // Validate the client's redirect_uris
        if (client.RedirectUris is null || client.RedirectUris.Count == 0)
        {
            logger.LogWarning("Client {ClientId} has no redirect_uris.", client.ClientId);
            return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidRedirectUri,
                OAuthErrorTypes.InvalidRedirectUri,
                "Invalid redirect_uri parameter.");
        }
        
        // Validate the requested redirect_uri
        var redirectUri = client.RedirectUris.FirstOrDefault(e => e.Uri == validateClientRequest.RedirectUri);
        if (redirectUri == null)
        {
            return GenericHttpResponse<ValidateClientResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OIdentErrors.InvalidRedirectUri,
                OAuthErrorTypes.InvalidRedirectUri,
                "Invalid redirect_uri parameter.");
        }
        
        // Create the response object
        var response = new ValidateClientResponse
        {
            ClientId = client.ClientId,
            ClientName = client.Name!,
            ClientRedirectUri = client.RedirectUris.FirstOrDefault(
                e => e.Uri == validateClientRequest.RedirectUri),
        };
        
        // Return the response
        return GenericHttpResponse<ValidateClientResponse>.CreateSuccessResponseWithData(
            HttpStatusCode.OK, 
            response);
    }
}