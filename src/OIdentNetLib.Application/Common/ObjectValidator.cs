using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Infrastructure.Errors;

namespace OIdentNetLib.Application.Common;

/// <summary>
/// Uses the .NET built-in object validation to check that
/// property values meet those requirements.
/// </summary>
public static class ObjectValidator
{
    public enum ObjectValidatorResultType
    {
        MultiLine,
        SingleLine
    }
    
    public static ObjectValidatorResults Validate(
        [NotNull] object? instance, 
        ObjectValidatorResultType resultType = ObjectValidatorResultType.MultiLine)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(instance);

        var isValid = Validator.TryValidateObject(
            instance,
            validationContext,
            validationResults,
            true);

        return new ObjectValidatorResults(isValid, validationResults);
    }
    
    public static GenericHttpResponse<object> ValidateObject(
        [NotNull] object? instance,
        ObjectValidatorResultType resultType = ObjectValidatorResultType.MultiLine)
    {
        // Get object validation results
        var objectValidationResults = Validate(instance);
        
        if (objectValidationResults.IsValid)
            return GenericHttpResponse<object>.CreateSuccessResponse(HttpStatusCode.OK);
        
        var errorMessage = new StringBuilder();
        bool isFirstPass = true;
        foreach(var validationResult in objectValidationResults.ValidationResults)
        {
            if (string.IsNullOrEmpty(validationResult.ErrorMessage))
                continue;

            switch (resultType)
            {
                case ObjectValidatorResultType.MultiLine:
                    errorMessage.AppendLine(validationResult.ErrorMessage);
                    break;
                case ObjectValidatorResultType.SingleLine:
                    if (!isFirstPass)
                        errorMessage.Append(", ");
                    errorMessage.Append(validationResult.ErrorMessage);
                    break;
            }
            isFirstPass = false;
        }
            
        return GenericHttpResponse<object>.CreateErrorResponse(
            HttpStatusCode.BadRequest,
            OIdentErrors.InvalidRequest,
            OAuthErrorTypes.InvalidRequest,
            errorMessage.ToString());
    }
}

