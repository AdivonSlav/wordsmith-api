namespace Wordsmith.Integration.MerriamWebster.Models;

public class DictionaryEntry
{
    public int Homograph { get; set; }
    
    public string? Date { get; set; }

    public DictionaryHeadword Headword { get; set; }
    
    public string? FunctionalLabel { get; set; }

    public ICollection<string> ShortDefs { get; set; }
    
    public string? Etymology { get; set; }
}