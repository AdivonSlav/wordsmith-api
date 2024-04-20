using MerriamWebster.NET;
using MerriamWebster.NET.Results;
using Wordsmith.Integration.MerriamWebster.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.Integration.MerriamWebster;

public class MerriamWebsterService : IMerriamWebsterService
{
    private readonly MerriamWebsterSearch _merriamWebsterSearch;
    
    public MerriamWebsterService(MerriamWebsterSearch merriamWebsterSearch)
    {
        _merriamWebsterSearch = merriamWebsterSearch;
    }
    
    public async Task<QueryResult<DictionaryResponse>> GetResults(string searchTerm)
    {
        var result = await _merriamWebsterSearch.SearchCollegiateDictionary(searchTerm);

        return new QueryResult<DictionaryResponse>()
        {
            Result = new List<DictionaryResponse>() { MapResultToDictionaryResponse(result) }
        };
    }

    private static DictionaryResponse MapResultToDictionaryResponse(ResultModel model)
    {
        var response = new DictionaryResponse
        {
            SearchTerm = model.SearchText
        };

        foreach (var entry in model.Entries)
        {
            var headword = new DictionaryHeadword()
            {
                Text = entry.Headword.Text
            };
            
            if (entry.Headword.Pronunciations != null)
            {
                foreach (var pronunciation in entry.Headword.Pronunciations)
                {
                    headword.Pronunciations.Add(new DictionaryPronunciation()
                    {
                        WrittenPronunciation = pronunciation.WrittenPronunciation,
                        LabelBefore = pronunciation.LabelBeforePronunciation,
                        LabelAfter = pronunciation.LabelAfterPronunciation,
                        Punctuation = pronunciation.Punctuation,
                    });
                }
            }
            
            response.Entries.Add(new DictionaryEntry()
            {
                Homograph = entry.Homograph,
                Date = entry.Date?.Text,
                FunctionalLabel = entry.FunctionalLabel?.Text,
                Headword = headword,
                Etymology = entry.Etymology?.Text.HtmlText,
                ShortDefs = entry.ShortDefs
            });
        }

        return response;
    }
}