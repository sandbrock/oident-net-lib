using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IAuthorizationSessionValidator
{
    Task<GenericHttpResponse<ValidateSessionResponse>> ValidateAsync(ValidateSessionRequest validateSessionRequest);
}