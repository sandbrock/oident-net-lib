using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface ITokenSessionProcessor
{
    Task<GenericHttpResponse<ProcessTokenSessionResponse>> ProcessAsync(
        ProcessTokenSessionRequest processTokenSessionRequest);
}