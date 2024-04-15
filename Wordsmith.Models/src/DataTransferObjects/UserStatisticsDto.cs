using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class UserStatisticsDto
{
    [SwaggerSchema("The number of books the user has published")]
    public int PublishedBooksCount { get; set; }
    
    [SwaggerSchema("The number of favorite books the user has")]
    public int FavoriteBooksCount { get; set; }
}