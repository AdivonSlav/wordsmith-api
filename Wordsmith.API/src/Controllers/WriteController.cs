using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

public class WriteController<T, TDb, TSearch, TInsert, TUpdate> : ReadController<T, TSearch>
    where T : class
    where TDb : class
    where TSearch : SearchObject
    where TInsert : class
    where TUpdate : class
{
    protected readonly IWriteService<T, TDb, TSearch, TInsert, TUpdate> WriteService; 

    public WriteController(IWriteService<T, TDb, TSearch, TInsert, TUpdate> writeService)
        : base(writeService)
    {
        WriteService = writeService;
    }

    [SwaggerOperation("Basic insert")]
    [HttpPost]
    public virtual async Task<ActionResult<EntityResult<T>>> Insert([FromBody] TInsert insert)
    {
        var userId = GetAuthUserId();
        return CreatedAtAction(nameof(Insert), await WriteService.Insert(insert, userId));
    }
    
    [SwaggerOperation("Basic update")]
    [HttpPut("{id:int}")]
    public virtual async Task<ActionResult<EntityResult<T>>> Update(int id, [FromBody] TUpdate update)
    {
        var userId = GetAuthUserId();
        return Ok(await WriteService.Update(id, update, userId));
    }
}