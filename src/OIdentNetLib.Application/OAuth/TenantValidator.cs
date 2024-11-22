using System.Net;
using Microsoft.Extensions.Logging;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.OAuth;

public class TenantValidator(
    ILogger<TenantValidator> logger,
    ITenantReader tenantReader
) : ITenantValidator
{
    public async Task<GenericHttpResponse<ValidateTenantResponse>> ValidateAsync(ValidateTenantRequest validateTenantRequest)
    {
        try
        {
            Tenant? tenant = null;
            if (!string.IsNullOrEmpty(validateTenantRequest.Path))
            {
                tenant = await tenantReader.GetByPathAsync(validateTenantRequest.Path);
            }
            else if (!string.IsNullOrEmpty(validateTenantRequest.Host))
            {
                tenant = await tenantReader.GetByHostAsync(validateTenantRequest.Host);
            }
            
            if (tenant == null)
            {
                return GenericHttpResponse<ValidateTenantResponse>.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    OIdentErrors.InvalidTenant,
                    OAuthErrorTypes.InvalidRequest,
                    "The tenant was not found.");
            }

            return GenericHttpResponse<ValidateTenantResponse>.CreateSuccessResponseWithData(
                HttpStatusCode.OK,
                new ValidateTenantResponse()
                {
                    Tenant = tenant
                });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating tenant with {Host} and {Path}.", 
                validateTenantRequest.Host, 
                validateTenantRequest.Path);
            return GenericHttpResponse<ValidateTenantResponse>.CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                OIdentErrors.InternalServerError,
                OAuthErrorTypes.ServerError,
                "A server error occurred while validating the tenant.");
        }
    }
}