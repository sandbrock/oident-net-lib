using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface IResourceServerValidator
{
    Task<GenericHttpResponse<ValidateResourceServerResponse>> ValidateAsync(
        RequestMetadata requestMetadata,
        ValidateResourceServerRequest validateResourceServerRequest);
}