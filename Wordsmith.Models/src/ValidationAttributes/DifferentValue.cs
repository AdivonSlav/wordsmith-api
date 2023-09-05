using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.ValidationAttributes;

/// <summary>
/// Validates that the two properties have different values
/// </summary>
public class DifferentValue : ValidationAttribute
{
    private readonly string _otherPropertyName;

    public DifferentValue(string otherPropertyName)
    {
        _otherPropertyName = otherPropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);

        if (otherProperty == null)
        {
            return new ValidationResult($"Property {_otherPropertyName} not found");
        }
        
        var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance);

        if (otherPropertyValue!.Equals(value))
        {
            return new ValidationResult(
                $"{validationContext.DisplayName} must have a different value to {_otherPropertyName}");
        }
        
        return ValidationResult.Success;
    }
}