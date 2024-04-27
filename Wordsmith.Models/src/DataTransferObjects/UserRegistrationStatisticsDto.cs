using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class UserRegistrationStatisticsDto
{
    [SwaggerSchema("Date of the statistics")]
    public DateTime Date { get; set; }
    
    [SwaggerSchema("How many users have registered for that month and year")]
    public long RegistrationCount { get; set; }
}