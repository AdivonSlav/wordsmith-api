using Wordsmith.Integration.MerriamWebster.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.Integration.MerriamWebster;

public interface IMerriamWebsterService
{
    Task<QueryResult<DictionaryResponse>> GetResults(string searchTerm);
}