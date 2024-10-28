using OIdentNetLib.Application.Authentication.DataTransferObjects;

namespace OIdentNetLib.Application.Authentication.Contracts;

public interface ILogoutProcessor
{
    Task<ProcessLogoutResponse> ProcessAsync(ProcessLogoutRequest processLogoutRequest);
}