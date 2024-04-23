using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Utils.StatisticsHelper;

public class StatisticsRequest
{
    [SwaggerSchema("Start date for statistics gathering")]
    public DateTime StartDate { get; set; }
    
    [SwaggerSchema("End date for statistics gathering")]
    public DateTime EndDate { get; set; }
}