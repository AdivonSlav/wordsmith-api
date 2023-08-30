using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Wordsmith.Models.ValidationAttributes;

/// <summary>
/// Validates that the passed password matches a set of rules
/// </summary>
public class Password : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null) return true;
        
        if (value is string password)
        {
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecialChar = new Regex(@"[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]").IsMatch(password);

            return hasDigit && hasSpecialChar;
        }

        return false;
    }
}