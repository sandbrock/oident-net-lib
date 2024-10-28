using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IAuthorizationCodeProcessor
{
    Task<ProcessAuthorizationCodeResponse> ProcessAsync(ProcessAuthoriziationCodeRequest processAuthoriziationCodeRequest);
}