using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Authorize("All")]
[Route("reports/reasons")]
public class ReportReasonsController : ReadController<ReportReasonDto, ReportReasonSearch>
{
    public ReportReasonsController(IReportReasonService reportReasonService)
        : base(reportReasonService) { }
}