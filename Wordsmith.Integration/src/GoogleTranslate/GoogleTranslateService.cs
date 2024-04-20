using System.Net;
using Google;
using Google.Cloud.Translation.V2;
using Microsoft.Extensions.Options;
using Wordsmith.Integration.GoogleTranslate.Models;
using Wordsmith.Models.Exceptions;
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

    public async Task<QueryResult<TranslationResponse>> Translate(TranslationRequest request)
    {
        try
        {
            var client = TranslationClient.CreateFromApiKey(_config.ApiKey);
            var translation = await client.TranslateTextAsync(request.Text, request.TargetLanguageCode, request.SourceLanguageCode);

            var translationResponse = new TranslationResponse()
            {
                OriginalLanguageCode = request.SourceLanguageCode != null ? translation.SpecifiedSourceLanguage : translation.DetectedSourceLanguage,
                OriginalText = translation.OriginalText,
                TranslatedLanguageCode = translation.TargetLanguage,
                Translation = translation.TranslatedText
            };

            return new QueryResult<TranslationResponse>()
            {
                Result = new List<TranslationResponse>() { translationResponse }
            };
            
        } catch (GoogleApiException e)
        {
            throw HandleGoogleException(e);
        }
    }

    public async Task<QueryResult<Language>> GetSupportedLanguages()
    {
        try
        {
            var client = TranslationClient.CreateFromApiKey(_config.ApiKey);
            var languages = await client.ListLanguagesAsync("en");

            var supportedLanguages = languages.Select(e => new Language()
            {
                Name = e.Name,
                LanguageCode = e.Code
            }).ToList();

            return new QueryResult<Language>()
            {
                Result = supportedLanguages
            };
        }
        catch (GoogleApiException e)
        {
            throw HandleGoogleException(e);
        }
    }

    private static Exception HandleGoogleException(GoogleApiException e)
    {
        return e.HttpStatusCode >= HttpStatusCode.InternalServerError ? new Exception(e.Message) : new AppException(e.Message);
    }
}