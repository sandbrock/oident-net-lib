using OIdentNetLib.Application.Authentication.Contracts;
using OIdentNetLib.Application.Authentication.DataTransferObjects;

namespace OIdentNetLib.Application.Authentication;

public class LogoutProcessor : ILogoutProcessor
{
    public async Task<ProcessLogoutResponse> ProcessAsync(ProcessLogoutRequest processLogoutRequest)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}