using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.AppReport;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.AppReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("app-reports")]
public class AppReportsController : WriteController<AppReportDto, AppReport, AppReportSearchObject, AppReportInsertRequest, AppReportUpdateRequest>
{
    public AppReportsController(IAppReportService appReportService) : base(appReportService) { }

    [SwaggerOperation("Get app reports based on a search criteria")]
    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<AppReportDto>>> Get(AppReportSearchObject search)
    {
        return base.Get(search);
    }

    [SwaggerOperation("Get an app report based on its ID")]
    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<AppReportDto>>> GetById(int id)
    {
        return base.GetById(id);
    }

    [SwaggerOperation("Add a new app report")]
    [Authorize("All")]
    public override Task<ActionResult<EntityResult<AppReportDto>>> Insert(AppReportInsertRequest insert)
    {
        insert.UserId = GetAuthUserId();
        return base.Insert(insert);
    }

    [SwaggerOperation("Update an existing app report")]
    [Authorize("AdminOperations")]
    public override Task<ActionResult<EntityResult<AppReportDto>>> Update(int id, AppReportUpdateRequest update)
    {
        return base.Update(id, update);
    }
}