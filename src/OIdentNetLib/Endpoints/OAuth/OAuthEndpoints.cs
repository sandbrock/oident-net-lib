using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Endpoints.Metadata;

namespace OIdentNetLib.Endpoints.OAuth;

public static class OAuthEndpoints
{
    public static void MapOAuthEndpoints(this WebApplication app)
    {
        app.MapGet("/oauth/authorize", AuthorizeAsync)
            .WithName("Authorize")
            .WithDescription("OAuth authorization endpoint.");
        
        app.MapPost("/oauth/token", TokenAsync)
            .WithName("Token")
            .WithDescription("OAuth token endpoint.");
    }

    private static async Task<IResult> AuthorizeAsync(
        HttpContext httpContext,
        HttpRequest httpRequest,
        IAuthorizationProcessor authorizationProcessor,
        [FromQuery] ProcessAuthorizationRequest processAuthorizationRequest)
    {
        var requestMetadata = RequestMetadataCreator.Create(httpRequest);
        var processAuthorizationResponse = await authorizationProcessor.ProcessAsync(
            requestMetadata,
            processAuthorizationRequest,
            new ValidateSessionRequest());
        return processAuthorizationResponse.ToHttpResult();
    }

    private static async Task<IResult> TokenAsync(
        HttpContext httpContext,
        HttpRequest httpRequest,
        ITokenProcessor tokenProcessor)
    {
        ProcessTokenRequest? processTokenRequest = null;

        if (httpRequest.HasJsonContentType())
        {
            processTokenRequest = await httpRequest.ReadFromJsonAsync<ProcessTokenRequest>();
        }
        else if (httpRequest.HasFormContentType)
        {
            var form = await httpRequest.ReadFormAsync();
            
            if (!Uri.TryCreate(form["redirect_uri"], UriKind.Absolute, out var redirectUri))
            {
                return Results.BadRequest(OAuthErrorTypes.InvalidRequest);
            }
            
            processTokenRequest = new ProcessTokenRequest()
            {
                ClientId = form["client_id"],
                ClientSecret = form["client_secret"],
                GrantType = form["grant_type"],
                Code = form["code"],
                RedirectUri = redirectUri
            };
        }
        
        if (processTokenRequest is null)
        {
            return Results.BadRequest(OAuthErrorTypes.InvalidRequest);
        }
        
        var requestMetadata = RequestMetadataCreator.Create(httpRequest);
        var processTokenResponse = await tokenProcessor.ProcessAsync(
            requestMetadata,
            processTokenRequest);
        return processTokenResponse.ToHttpResult();
    }
}