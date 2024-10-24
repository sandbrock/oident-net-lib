using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

public class RefreshTokenProcessor : IRefreshTokenProcessor
{
    public async Task<ProcessRefreshTokenResponse> ProcessAsync(ProcessRefreshTokenRequest request)
    {
        await Task.CompletedTask;
        
        throw new NotImplementedException();
    }
}