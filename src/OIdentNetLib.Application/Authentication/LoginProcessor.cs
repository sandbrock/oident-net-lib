using Microsoft.IdentityModel.Tokens;
using OIdentNetLib.Application.Authentication.Contracts;
using OIdentNetLib.Application.Authentication.DataTransferObjects;

namespace OIdentNetLib.Application.Authentication;

public class LoginProcessor : ILoginProcessor
{
    public async Task<ProcessLoginResponse> ProcessAsync(ProcessLoginRequest processLoginRequest)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}