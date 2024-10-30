using System.ComponentModel.DataAnnotations;

namespace OIdentNetLib.Application.Common;

/// <summary>
/// Contains a list of object validation results.
/// </summary>
public class ObjectValidatorResults(bool isValid, IEnumerable<ValidationResult> validationResults)
{
    public bool IsValid { get; } = isValid;

    public IEnumerable<ValidationResult> ValidationResults { get; } = validationResults;
}
