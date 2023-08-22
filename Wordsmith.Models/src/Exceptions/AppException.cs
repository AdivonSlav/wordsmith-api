namespace Wordsmith.Models.Exceptions;

/// <summary>
/// Represents a generic exception related to a bad request and is supposed to be returned with status 400
/// </summary>
public class AppException : Exception
{
    public AppException(string message)
        : base(message)
    {
        
    }

    public AppException(string message, Dictionary<string, string> additionalInfo)
        : base(message)
    {
        foreach (var pair in additionalInfo)
        {
            base.Data.Add(pair.Key, pair.Value);
        }
    }
}