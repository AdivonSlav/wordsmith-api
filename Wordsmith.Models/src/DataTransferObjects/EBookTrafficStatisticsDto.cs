using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class EBookTrafficStatisticsDto
{
    [SwaggerSchema("ID of the ebook")]
    public int Id { get; set; }
    
    [SwaggerSchema("Title of the ebook")]
    public string Title { get; set; }
    
    [SwaggerSchema("How many times the ebook has been synchronized to a user library")]
    public long SyncCount { get; set; }
}