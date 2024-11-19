using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace OIdentNetLib.Endpoints.OAuth;

public static class OAuthEndpoints
{
    public static void MapOAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/oauth/authorize", AuthorizeAsync);
        app.MapPost("/oauth/token", TokenAsync);
    }

    private static async Task<IResult> AuthorizeAsync(
        HttpContext context,
        HttpContent content)
    {
        await Task.CompletedTask;
        return Results.Ok();
    }

    private static async Task<IResult> TokenAsync(
        HttpContext context,
        HttpContent content)
    {
        await Task.CompletedTask;
        return Results.Ok();
    }
}