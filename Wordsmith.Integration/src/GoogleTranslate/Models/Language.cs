using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Integration.GoogleTranslate.Models;

public class Language
{
    [SwaggerSchema("Name of the language for display purposes")]
    public string Name { get; set; }
    
    [SwaggerSchema("The language code")]
    public string LanguageCode { get; set; }
}