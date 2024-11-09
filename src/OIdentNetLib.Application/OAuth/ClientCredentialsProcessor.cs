using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

/// <summary>
/// Processes the client_credentials OAuth flow.
/// </summary>
public class ClientCredentialsProcessor : IClientCredentialsProcessor
{
    public async Task<GenericHttpResponse<ProcessTokenResponse>> ProcessAsync(
        RequestMetadata requestMetadata,
        ProcessTokenRequest request)
    {
        await Task.CompletedTask;
        
        throw new NotImplementedException();
    }
}