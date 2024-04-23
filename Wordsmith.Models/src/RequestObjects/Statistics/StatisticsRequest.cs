using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.RequestObjects.Statistics;

public class StatisticsRequest
{
    [SwaggerParameter("Start date for statistics gathering")]
    [Required]
    public DateTime StartDate { get; set; }
    
    [SwaggerParameter("End date for statistics gathering")]
    [Required]
    public DateTime EndDate { get; set; }
    
    [SwaggerParameter("How many entities to take into account")]
    public int? Limit { get; set; }
}