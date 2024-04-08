using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.RequestObjects.EBookReport;

public class EBookReportEmailSendRequest
{
    [SwaggerSchema("ID of the report to send an email for")]
    public int ReportId { get; set; }
    
    [SwaggerSchema("Body of the email")]
    public string Body { get; set; }
}