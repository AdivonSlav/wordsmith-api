using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class UserRegistrationStatisticsDto
{
    [SwaggerSchema("Year of the registrations")]
    public int Year { get; set; }
    
    [SwaggerSchema("Month of the registrations")]
    public string Month { get; set; }
    
    [SwaggerSchema("How many users have registered for that month and year")]
    public long RegistrationCount { get; set; }
}