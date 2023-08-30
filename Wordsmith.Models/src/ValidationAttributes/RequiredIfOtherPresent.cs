using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.ValidationAttributes;

/// <summary>
/// Requires a property on an object if the other property passed also has a value
/// </summary>
public class RequiredIfOtherPresent : ValidationAttribute
{
    private readonly string _otherPropertyName;

    public RequiredIfOtherPresent(string otherPropertyName)
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

        if (otherPropertyValue != null && value == null)
        {
            return new ValidationResult(
                $"{validationContext.DisplayName} is required when {_otherPropertyName} has a value");
        }

        if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
        {
            return new ValidationResult(
                $"{validationContext.DisplayName} is required when {_otherPropertyName} has a value");
        }
        
        return ValidationResult.Success;;
    }
}