using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Wordsmith.Models.ValidationAttributes;

public class EpubFile : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return new ValidationResult("The value passed must be an IFormFile!");
        }

        if (file.Length <= 0) return new ValidationResult("The provided file must not be empty");
        if (file.ContentType != "application/epub+zip") return new ValidationResult("The provided file is not an EPUB");
        if (Path.GetExtension(file.FileName) != ".epub")
            return new ValidationResult("The provided file does not have the EPUB extension");
        
        return ValidationResult.Success;
    }
}