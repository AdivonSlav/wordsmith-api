using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class EBookRatingStatisticsDto
{
    [SwaggerSchema("The ID of the book")]
    public int EBookId { get; set; }
    
    [SwaggerSchema("The average rating for the ebook")]
    public double? RatingAverage { get; set; }

    [SwaggerSchema("Total count of ratings")]
    public int TotalRatingCount { get; set; }
    
    [SwaggerSchema("The rating counts where the keys are ratings 1-5 and the values are the counts of the respective ratings")]
    public Dictionary<int, int> RatingCounts { get; init; }
}