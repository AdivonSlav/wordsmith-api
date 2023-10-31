using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("ebook-reports")]
public class EBookReportsController : WriteController<EBookReportDto, EBookReport, EBookReportSearchObject, EBookReportInsertRequest, EBookReportUpdateRequest>
{
    public EBookReportsController(
        IWriteService<EBookReportDto, EBookReport, EBookReportSearchObject, EBookReportInsertRequest,
            EBookReportUpdateRequest> writeService) : base(writeService) { }

    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<EBookReportDto>>> Get(EBookReportSearchObject? search = null)
    {
        return base.Get(search);
    }

    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<EBookReportDto>>> GetById(int id)
    {
        return base.GetById(id);
    }

    [Authorize("All")]
    public override Task<ActionResult<EBookReportDto>> Insert(EBookReportInsertRequest insert)
    {
        return base.Insert(insert);
    }

    [Authorize("AdminOperations")]
    public override Task<ActionResult<EBookReportDto>> Update(int id, EBookReportUpdateRequest update)
    {
        return base.Update(id, update);
    }
}