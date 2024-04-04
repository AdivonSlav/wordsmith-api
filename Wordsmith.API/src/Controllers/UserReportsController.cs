using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.User;
using Wordsmith.DataAccess.Services.UserReport;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.UserReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-reports")]
public class UserReportsController : WriteController<UserReportDto, UserReport, UserReportSearchObject,
    UserReportInsertRequest, UserReportUpdateRequest>
{
    public UserReportsController(IUserReportService userReportService, IUserService userService)
        : base(userReportService) { }

    [SwaggerOperation("Get user reports based on a search criteria")]
    [Authorize("AdminOperations")]
    public override async Task<ActionResult<QueryResult<UserReportDto>>> Get(UserReportSearchObject search)
    {
        return await base.Get(search);
    }

    [SwaggerOperation("Get a user report based on its ID")]
    [Authorize("AdminOperations")]
    public override async Task<ActionResult<QueryResult<UserReportDto>>> GetById(int id)
    {
        return await base.GetById(id);
    }

    [SwaggerOperation("Add a new user report")]
    [Authorize("All")]
    public override async Task<ActionResult<EntityResult<UserReportDto>>> Insert(UserReportInsertRequest insert)
    {
        insert.ReporterUserId = GetAuthUserId();   
        return await base.Insert(insert);
    }

    [SwaggerOperation("Update an existing user report")]
    [Authorize("AdminOperations")]
    public override async Task<ActionResult<EntityResult<UserReportDto>>> Update(int id, UserReportUpdateRequest update)
    {
        return await base.Update(id, update);
    }
}