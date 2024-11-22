using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Processes the refresh_token OAuth flow
/// </summary>
public class RefreshTokenProcessor(
    ILogger<RefreshTokenProcessor> logger,
    ITenantValidator tenantValidator,
    ITokenSessionValidator tokenSessionValidator
) : IRefreshTokenProcessor
{
    public async Task<GenericHttpResponse<ProcessTokenResponse>> ProcessAsync(
        RequestMetadata requestMetadata,
        ProcessTokenRequest request)
    {
        await Task.CompletedTask;
        
        throw new NotImplementedException();
    }
}