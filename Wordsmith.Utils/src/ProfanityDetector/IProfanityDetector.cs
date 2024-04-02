namespace Wordsmith.Utils.ProfanityDetector;

/// <summary>
/// An abstraction over the ProfanityDetector library. Used to check string values against a profanity list and censor them
/// </summary>
public interface IProfanityDetector
{
    /// <summary>
    /// Checks whether the passed string contains profanity
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <returns>True if the string contains profanity or False</returns>
    bool ContainsProfanity(string value);
    
    /// <summary>
    /// Replaces any profanity with '*' within the passed string
    /// </summary>
    /// <param name="value">The value to check</param>
    /// <returns>The censored value</returns>
    string Censor(string value);
}