using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Integration.GoogleTranslate.Models;

public class TranslationRequest
{
    [Required]
    [SwaggerParameter("Text to translate")]
    public string Text { get; set; }
    
    [SwaggerParameter("Language code of the source language")]
    public string? SourceLanguageCode { get; set; }
    
    [Required]
    [SwaggerParameter("Language code of the target language")]
    public string TargetLanguageCode { get; set; }
}