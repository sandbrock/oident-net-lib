using Microsoft.AspNetCore.Builder;
using OIdentNetLib.Endpoints.OAuth;

namespace OIdentNetLib;

public static class EndpointRegistration
{
    public static void MapOIdentNetLibEndpoints(this WebApplication app)
    {
        app.MapOAuthEndpoints();
    }
}