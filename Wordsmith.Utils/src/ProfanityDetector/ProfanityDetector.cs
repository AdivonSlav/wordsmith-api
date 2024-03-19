namespace Wordsmith.Utils.ProfanityDetector;

public class ProfanityDetector : IProfanityDetector
{
    private readonly ProfanityFilter.ProfanityFilter _filter = new();

    public bool ContainsProfanity(string value)
    {
        return _filter.ContainsProfanity(value);
    }

    public string Censor(string value)
    {
        return _filter.CensorString(value, '*');
    }
}