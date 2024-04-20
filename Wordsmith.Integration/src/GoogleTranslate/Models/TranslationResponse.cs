using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Integration.GoogleTranslate.Models;

public class TranslationResponse
{
    [SwaggerSchema("The original text sent for translation")]
    public string OriginalText { get; set; }
    
    [SwaggerSchema("The translated text")]
    public string Translation { get; set; }
    
    [SwaggerSchema("The original language of the text")]
    public string OriginalLanguageCode { get; set; }
    
    [SwaggerSchema("The translation language")]
    public string TranslatedLanguageCode { get; set; }
}