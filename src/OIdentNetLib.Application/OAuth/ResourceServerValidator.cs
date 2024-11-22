using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth;

public class ResourceServerValidator : IResourceServerValidator
{
    public async Task<GenericHttpResponse<ValidateResourceServerResponse>> ValidateAsync(
        RequestMetadata requestMetadata,
        ValidateResourceServerRequest validateResourceServerRequest)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}