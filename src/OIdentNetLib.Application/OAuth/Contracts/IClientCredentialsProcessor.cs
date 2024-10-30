using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IClientCredentialsProcessor
{
    Task<ProcessTokenResponse> ProcessAsync(ProcessTokenRequest request);
}