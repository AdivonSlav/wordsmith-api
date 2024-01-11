using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-reports")]
public class UserReportsController : WriteController<UserReportDto, UserReport, UserReportSearchObject,
    UserReportInsertRequest, UserReportUpdateRequest>
{
    protected IUserService UserService;

    public UserReportsController(IUserReportService userReportService, IUserService userService)
        : base(userReportService)
    {
        UserService = userService;
    }

    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<UserReportDto>>> Get(UserReportSearchObject? search = null)
    {
        return base.Get(search);
    }

    [Authorize("AdminOperations")]
    public override Task<ActionResult<QueryResult<UserReportDto>>> GetById(int id)
    {
        return base.GetById(id);
    }

    [Authorize("All")]
    public override async Task<ActionResult<UserReportDto>> Insert(UserReportInsertRequest insert)
    {
        var user = await UserService.GetUserFromClaims(HttpContext.User.Claims);
        insert.ReporterUserId = user.Id;
        
        return await base.Insert(insert);
    }

    [Authorize("AdminOperations")]
    public override Task<ActionResult<UserReportDto>> Update(int id, UserReportUpdateRequest update)
    {
        return base.Update(id, update);
    }
}