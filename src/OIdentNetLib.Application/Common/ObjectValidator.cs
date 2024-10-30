using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using OIdentNetLib.Application.OAuth.Models;

namespace OIdentNetLib.Application.Common;

/// <summary>
/// Uses the .NET built-in object validation to check that
/// property values meet those requirements.
/// </summary>
public static class ObjectValidator
{
    public static ObjectValidatorResults Validate([NotNull] object? instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(instance);

        bool isValid = Validator.TryValidateObject(
            instance,
            validationContext,
            validationResults,
            true);

        return new ObjectValidatorResults(isValid, validationResults);
    }
    
    public static GenericHttpResponse<object> ValidateObject([NotNull] object? instance)
    {
        // Get object validation results
        var objectValidationResults = Validate(instance);
        
        if (objectValidationResults.IsValid)
            return GenericHttpResponse<object>.CreateSuccessResponse(HttpStatusCode.OK);
        
        var errorMessage = new StringBuilder();
        foreach(var validationResult in objectValidationResults.ValidationResults)
        {
            if (string.IsNullOrEmpty(validationResult.ErrorMessage))
                continue;

            errorMessage.AppendLine(validationResult.ErrorMessage);
        }
            
        return GenericHttpResponse<object>.CreateErrorResponse(
            HttpStatusCode.BadRequest,
            OAuthErrorTypes.InvalidRequest,
            errorMessage.ToString());
    }
}
