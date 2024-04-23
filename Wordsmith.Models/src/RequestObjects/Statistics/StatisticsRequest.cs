using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.RequestObjects.Statistics;

public class StatisticsRequest
{
    [SwaggerParameter("Start date for statistics gathering")]
    public DateTime StartDate { get; set; }
    
    [SwaggerParameter("End date for statistics gathering")]
    public DateTime EndDate { get; set; }
}