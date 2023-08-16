using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[Route("report/reasons")]
public class ReportReasonController : ReadController<Models.ReportReasonDTO, SearchObject>
{
    public ReportReasonController(IReadService<ReportReasonDTO, SearchObject> readService)
        : base(readService)
    {
        
    }
}
