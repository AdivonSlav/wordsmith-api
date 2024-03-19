namespace Wordsmith.Utils.ProfanityDetector;

public interface IProfanityDetector
{
    bool ContainsProfanity(string value);
    string Censor(string value);
}