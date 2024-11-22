using Microsoft.AspNetCore.Http;
using OIdentNetLib.Application.Common;

namespace OIdentNetLib.Endpoints.Metadata;

public static class RequestMetadataCreator
{
    public static RequestMetadata Create(HttpContext httpContext)
    {
        string? host = httpContext.Request.Headers["X-Forwarded-For"];
        if (string.IsNullOrEmpty(host))
        {
            host = httpContext.Request.Host.Value;
        }

        var tenantPath = httpContext.Items.TryGetValue("tenant_path", out var path)
            ? path!.ToString()
            : null;
        
        return new RequestMetadata()
        {
            Host = host,
            TenantPath = tenantPath
        };
    }
}