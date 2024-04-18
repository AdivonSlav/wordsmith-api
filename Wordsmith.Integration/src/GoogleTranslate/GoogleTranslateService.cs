using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Options;
using Wordsmith.Integration.GoogleTranslate.Models;
using Wordsmith.Models.SearchObjects;
using Language = Wordsmith.Integration.GoogleTranslate.Models.Language;

namespace Wordsmith.Integration.GoogleTranslate;

public class GoogleTranslateService : IGoogleTranslateService
{
    private readonly GoogleTranslateConfig _config;
    
    public GoogleTranslateService(IOptions<GoogleTranslateConfig> config)
    {
        _config = config.Value; 
    }
    
    public async Task<QueryResult<SupportedLanguages>> GetSupportedLanguages()
    {
        var client = TranslationClient.CreateFromApiKey(_config.ApiKey);
        var languages = await client.ListLanguagesAsync("en");

        var supportedLanguages = new SupportedLanguages()
        {
            Languages = languages.Select(e => new Language()
            {
                Name = e.Name,
                LanguageCode = e.Code
            })
        };

        return new QueryResult<SupportedLanguages>()
        {
            Result = new List<SupportedLanguages>() { supportedLanguages } 
        };
    }
}