namespace Wordsmith.Models.DataTransferObjects;

public class EntityResult<T> where T : class
{
    public string? Message { get; set; }
    
    public T? Result { get; set; }
}