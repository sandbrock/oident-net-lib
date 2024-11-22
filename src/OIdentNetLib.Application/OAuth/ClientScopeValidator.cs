using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

public class ClientScopeValidator : IClientScopeValidator
{
    public async Task<ValidateClientScopeResponse> ValidateAsync(ValidateClientScopeRequest validateClientScopeRequest)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}