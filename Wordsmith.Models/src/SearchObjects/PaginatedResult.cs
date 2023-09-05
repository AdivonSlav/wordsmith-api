namespace Wordsmith.Models.SearchObjects;

public class PaginatedResult<T>
{
    public List<T> Result { get; set; }
    public int? Page { get; set; }
    public int? TotalPages { get; set; }
    public int? TotalCount { get; set; }
}