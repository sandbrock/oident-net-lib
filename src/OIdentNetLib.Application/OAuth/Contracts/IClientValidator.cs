using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IClientValidator
{
    Task<GenericHttpResponse<ValidateClientResponse>> ValidateAsync(ValidateClientRequest validateClientRequest);
}