using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface ITokenProcessor
{
    Task<GenericHttpResponse<ProcessTokenResponse>> ProcessAsync(
        RequestMetadata host, 
        ProcessTokenRequest processTokenRequest);
}