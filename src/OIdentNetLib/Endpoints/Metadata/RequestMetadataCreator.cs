using Microsoft.AspNetCore.Http;
using OIdentNetLib.Application.Common;

namespace OIdentNetLib.Endpoints.Metadata;

public static class RequestMetadataCreator
{
    public static RequestMetadata Create(HttpRequest httpRequest)
    {
        string? host = httpRequest.Headers["X-Forwarded-For"];
        if (string.IsNullOrEmpty(host))
        {
            host = httpRequest.Host.Value;
        }
        
        return new RequestMetadata()
        {
            Host = host
        };
    }
}