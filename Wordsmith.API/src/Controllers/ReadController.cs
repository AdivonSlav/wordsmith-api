using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

public class ReadController<T, TSearch> : ControllerBase
    where T : class
    where TSearch : SearchObject
{
    protected readonly IReadService<T, TSearch> ReadService;

    public ReadController(IReadService<T, TSearch> readService)
    {
        ReadService = readService;
    }

    [SwaggerOperation("Basic search")]
    [HttpGet()]
    public virtual async Task<ActionResult<QueryResult<T>>> Get([FromQuery] TSearch search)
    {
        var userId = GetAuthUserId();
        return Ok(await ReadService.Get(search, userId));
    }

    [SwaggerOperation("Search by ID")]
    [HttpGet("{id:int}")]
    public virtual async Task<ActionResult<QueryResult<T>>> GetById(int id)
    {
        var userId = GetAuthUserId();
        return Ok(await ReadService.GetById(id, userId));
    }

    /// <summary>
    /// Retrieves the user that made the request based on the provided authorization header
    /// </summary>
    /// <returns>The ID of the user that made the request or -1 if no user claims were found</returns>
    [NonAction]
    protected int GetAuthUserId()
    {
        var userRefId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_ref_id");

        if (userRefId != null && int.TryParse(userRefId.Value, out var userId))
        {
            return userId;  
        }

        return -1;
    }
}