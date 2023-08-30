using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.ValidationAttributes;

/// <summary>
/// Validates that the passed Base64 encoded image will not exceed a maximum byte size when decoded
/// </summary>
public class Base64Image : ValidationAttribute
{
    private const long MaxEncodedBytes = 10 * 1048576; // 10MB

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string encodedImage)
        {
            const int bitsEncodedPerChar = 6;
            var bytesExpected = (encodedImage.Length * bitsEncodedPerChar) >> 3; // Dividing by 8 to get the bytes

            return bytesExpected <= MaxEncodedBytes
                ? ValidationResult.Success
                : new ValidationResult("Passed Base64 string exceeds expected byte size");
        }

        return new ValidationResult("Not a string");
    }
}