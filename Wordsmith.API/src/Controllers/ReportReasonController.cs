using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[Route("report/reasons")]
public class ReportReasonController : ReadController<ReportReasonDto, SearchObject>
{
    public ReportReasonController(IReadService<ReportReasonDto, SearchObject> readService)
        : base(readService) { }
}