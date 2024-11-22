using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IClientScopeValidator
{
    Task<ValidateClientScopeResponse> ValidateAsync(ValidateClientScopeRequest validateClientScopeRequest);
}