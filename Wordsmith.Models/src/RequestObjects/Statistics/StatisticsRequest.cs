using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.Models.RequestObjects.Statistics;

public class StatisticsRequest : SearchObject
{
    [SwaggerParameter("Start date for statistics gathering")]
    [Required]
    public DateTime StartDate { get; set; }
    
    [SwaggerParameter("End date for statistics gathering")]
    [Required]
    public DateTime EndDate { get; set; }
}