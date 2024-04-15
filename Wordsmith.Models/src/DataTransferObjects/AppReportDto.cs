using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class AppReportDto
{
    [SwaggerSchema("ID of the report")]
    public int Id { get; set; }
    
    [SwaggerSchema("Content of the report")]
    public string Content { get; set; }
    
    [SwaggerSchema("User who made the report")]
    public UserDto User { get; set; }
    
    [SwaggerSchema("Whether the report is closed or not")]
    public bool IsClosed { get; set; }
    
    [SwaggerSchema("Date when the report was made")]
    public DateTime SubmissionDate { get; set; }
}