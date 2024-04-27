using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class EBookPublishStatisticsDto
{
    [SwaggerSchema("Date of the statistics")]
    public DateTime Date { get; set; }
    
    [SwaggerSchema("How many ebooks have been published for that month and year")]
    public long PublishCount { get; set; }
}