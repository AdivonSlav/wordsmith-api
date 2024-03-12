namespace Wordsmith.Models.SearchObjects;

public class QueryResult<T> where T : class
{
    public List<T> Result { get; set; } = new();
    public int? Page { get; set; }
    public int? TotalPages { get; set; }
    public int? TotalCount { get; set; }
}