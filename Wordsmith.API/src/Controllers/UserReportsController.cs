using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-reports")]
public class UserReportsController : WriteController<UserReportDto, UserReport, UserReportSearchObject,
    UserReportInsertRequest, UserReportUpdateRequest>
{
    public UserReportsController(IUserReportService userReportService)
        : base(userReportService) { }

    [Authorize("AdminOperations")]
    public override Task<QueryResult<UserReportDto>> Get(UserReportSearchObject? search = null)
    {
        return base.Get(search);
    }

    [Authorize("AdminOperations")]
    public override Task<UserReportDto> GetById(int id)
    {
        return base.GetById(id);
    }

    [Authorize("All")]
    public override Task<ActionResult<UserReportDto>> Insert(UserReportInsertRequest insert)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_ref_id");
        insert.ReporterUserId = userId?.Value != null ? int.Parse(userId.Value) : null;

        return base.Insert(insert);
    }

    [Authorize("AdminOperations")]
    public override Task<ActionResult<UserReportDto>> Update(int id, UserReportUpdateRequest update)
    {
        return base.Update(id, update);
    }
}