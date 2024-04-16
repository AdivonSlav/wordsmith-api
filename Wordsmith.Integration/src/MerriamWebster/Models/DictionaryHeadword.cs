namespace Wordsmith.Integration.MerriamWebster.Models;

public class DictionaryHeadword
{
    public string? Text { get; set; }

    public ICollection<DictionaryPronunciation> Pronunciations { get; set; } = new List<DictionaryPronunciation>();
}