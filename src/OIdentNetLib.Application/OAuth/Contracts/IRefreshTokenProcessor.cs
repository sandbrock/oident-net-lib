using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IRefreshTokenProcessor
{
    Task<ProcessRefreshTokenResponse> ProcessAsync(ProcessRefreshTokenRequest request);
}