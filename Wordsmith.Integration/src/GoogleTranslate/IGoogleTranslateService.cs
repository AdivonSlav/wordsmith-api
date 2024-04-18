using Wordsmith.Integration.GoogleTranslate.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.Integration.GoogleTranslate;

public interface IGoogleTranslateService
{
    /// <summary>
    /// Translates the given text 
    /// </summary>
    /// <param name="request">Translation request which contains the text, source and target languages</param>
    /// <returns>The translation response</returns>
    Task<QueryResult<TranslationResponse>> Translate(TranslationRequest request);
    
    /// <summary>
    /// Gets the supported languages for translation
    /// </summary>
    /// <returns>The supported languages response</returns>
    Task<QueryResult<SupportedLanguages>> GetSupportedLanguages();
}