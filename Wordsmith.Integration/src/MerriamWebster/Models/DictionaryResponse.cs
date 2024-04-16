namespace Wordsmith.Integration.MerriamWebster.Models;

public class DictionaryResponse
{
    public string SearchTerm { get; set; }

    public ICollection<DictionaryEntry> Entries { get; set; } = new List<DictionaryEntry>();
}