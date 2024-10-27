using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IAuthorizationProcessor
{
    Task<GenericHttpResponse<ProcessAuthorizationResponse>> ProcessAsync(
        ProcessAuthorizationRequest processAuthorizationRequest,
        ValidateSessionRequest validateSessionRequest);
}