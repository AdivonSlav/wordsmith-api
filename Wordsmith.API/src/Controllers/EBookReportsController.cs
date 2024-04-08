using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.EBookReport;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBookReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("ebook-reports")]
public class EBookReportsController : WriteController<EBookReportDto, EBookReport, EBookReportSearchObject, EBookReportInsertRequest, EBookReportUpdateRequest>
{
    public EBookReportsController(IEBookReportService eBookReportService) : base(eBookReportService) { }

    [SwaggerOperation("Gets reports for ebooks based on a search criteria")]
    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<EBookReportDto>>> Get(EBookReportSearchObject search)
    {
        return base.Get(search);
    }
    
    [SwaggerOperation("Gets an ebook report based on its ID")]
    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<EBookReportDto>>> GetById(int id)
    {
        return base.GetById(id);
    }

    [SwaggerOperation("Adds a new ebook report")]
    [Authorize("All")]
    public override Task<ActionResult<EntityResult<EBookReportDto>>> Insert(EBookReportInsertRequest insert)
    {
        insert.ReporterUserId = GetAuthUserId();
        return base.Insert(insert);
    }

    [SwaggerOperation("Updates an ebook report")]
    [Authorize("AdminOperations")]
    public override Task<ActionResult<EntityResult<EBookReportDto>>> Update(int id, EBookReportUpdateRequest update)
    {
        return base.Update(id, update);
    }
    
    [SwaggerOperation("Send an email to the author of the reported ebook")]
    [Authorize("AdminOperations")]
    [HttpPost("email")]
    public async Task<ActionResult<EntityResult<EBookReportDto>>> SendEmail(EBookReportEmailSendRequest request)
    {
        return Ok(await (WriteService as IEBookReportService)!.SendEmail(request));
    }
}