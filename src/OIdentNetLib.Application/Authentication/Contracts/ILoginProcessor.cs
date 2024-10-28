using OIdentNetLib.Application.Authentication.DataTransferObjects;

namespace OIdentNetLib.Application.Authentication.Contracts;

public interface ILoginProcessor
{
    Task<ProcessLoginResponse> ProcessAsync(ProcessLoginRequest processLoginRequest);
}