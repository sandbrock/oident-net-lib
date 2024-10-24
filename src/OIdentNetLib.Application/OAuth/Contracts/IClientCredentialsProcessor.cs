using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IClientCredentialsProcessor
{
    Task<ProcessClientCredentialsResponse> ProcessAsync(ProcessClientCredentialsRequest request);
}