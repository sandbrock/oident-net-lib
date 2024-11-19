using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface ITokenSessionCreator
{
    Task<GenericHttpResponse<CreateTokenSessionResponse>> CreateAsync(
        CreateTokenSessionRequest createTokenSessionRequest);
}