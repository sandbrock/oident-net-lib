using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IScopeValidator
{
    Task<GenericHttpResponse<ValidateScopeResponse>> ValidateAsync(ValidateScopeRequest validateScopeRequest);
}