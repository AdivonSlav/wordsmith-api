using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class EBookPublishStatisticsDto
{
    [SwaggerSchema("Year of the registrations")]
    public int Year { get; set; }
    
    [SwaggerSchema("Month of the registrations")]
    public int Month { get; set; }
    
    [SwaggerSchema("How many users have registered for that month and year")]
    public long PublishCount { get; set; }
}