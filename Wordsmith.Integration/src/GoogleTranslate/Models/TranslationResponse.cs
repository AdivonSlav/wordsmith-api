namespace Wordsmith.Integration.GoogleTranslate.Models;

public class TranslationResponse
{
    public string OriginalText { get; set; }
    
    public string Translation { get; set; }
    
    public string OriginalLanguageCode { get; set; }
    
    public string TranslatedLanguageCode { get; set; }
}