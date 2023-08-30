namespace Wordsmith.Models.MessageObjects;

public class OperationStatusMessage
{
    public bool Succeeded { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}