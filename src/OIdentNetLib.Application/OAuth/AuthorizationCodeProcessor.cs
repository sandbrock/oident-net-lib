using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

public class AuthorizationCodeProcessor : IAuthorizationCodeProcessor
{
    public async Task<ProcessAuthorizationCodeResponse> ProcessAsync(ProcessAuthoriziationCodeRequest processAuthoriziationCodeRequest)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}