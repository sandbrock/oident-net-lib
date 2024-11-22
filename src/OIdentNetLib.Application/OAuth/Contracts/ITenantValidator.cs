using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.DataTransferObjects;

namespace OIdentNetLib.Application.OAuth.Contracts;

public interface ITenantValidator
{
    Task<GenericHttpResponse<ValidateTenantResponse>> ValidateAsync(ValidateTenantRequest validateTenantRequest);
}