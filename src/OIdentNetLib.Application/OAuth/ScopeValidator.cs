using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

public class ScopeValidator : IScopeValidator
{
    public async Task<GenericHttpResponse<ValidateScopeResponse>> ValidateAsync(ValidateScopeRequest validateScopeRequest)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}