using Wordsmith.Integration.MerriamWebster.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.Integration.MerriamWebster;

public interface IMerriamWebsterService
{
    /// <summary>
    /// Gets dictionary entries for the given search term
    /// </summary>
    /// <param name="searchTerm">The term to search for</param>
    /// <returns>The dictionary response</returns>
    Task<QueryResult<DictionaryResponse>> GetResults(string searchTerm);
}