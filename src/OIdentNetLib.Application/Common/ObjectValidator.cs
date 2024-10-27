using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OIdentNetLib.Application.Common;

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
}
