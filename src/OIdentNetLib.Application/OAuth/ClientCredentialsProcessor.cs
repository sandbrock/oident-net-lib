using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

public class ClientCredentialsProcessor : IClientCredentialsProcessor
{
    public async Task<ProcessClientCredentialsResponse> ProcessAsync(ProcessClientCredentialsRequest request)
    {
        await Task.CompletedTask;
        
        throw new NotImplementedException();
    }
}