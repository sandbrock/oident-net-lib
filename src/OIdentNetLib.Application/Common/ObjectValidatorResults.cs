using System.ComponentModel.DataAnnotations;

namespace OIdentNetLib.Application.Common;

public class ObjectValidatorResults(bool isValid, IEnumerable<ValidationResult> validationResults)
{
    public bool IsValid { get; } = isValid;

    public IEnumerable<ValidationResult> ValidationResults { get; } = validationResults;
}
