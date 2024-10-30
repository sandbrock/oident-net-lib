using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IAuthorizationCodeProcessor
{
    Task<GenericHttpResponse<ProcessTokenResponse>> ProcessAsync(ProcessTokenRequest processTokenRequest);
}