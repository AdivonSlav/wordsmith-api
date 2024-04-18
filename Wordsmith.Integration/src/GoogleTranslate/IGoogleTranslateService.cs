using Wordsmith.Integration.GoogleTranslate.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.Integration.GoogleTranslate;

public interface IGoogleTranslateService
{
    Task<QueryResult<TranslationResponse>> Translate(TranslationRequest request);
    Task<QueryResult<SupportedLanguages>> GetSupportedLanguages();
}