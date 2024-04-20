namespace Wordsmith.Integration.MerriamWebster.Models;

public class DictionaryPronunciation
{
    public string WrittenPronunciation { get; set; }
    
    public string? LabelBefore { get; set; }
    
    public string? LabelAfter { get; set; }
    
    public string? Punctuation { get; set; }
}