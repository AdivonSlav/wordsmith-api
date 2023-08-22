using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
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

    [HttpPost]
    public virtual async Task<ActionResult<T>> Insert([FromBody] TInsert insert)
    {
        return await WriteService.Insert(insert);
    }
    
    [HttpPut("{id:int}")]
    public virtual async Task<ActionResult<T>> Update(int id, [FromBody] TUpdate update)
    {
        return await WriteService.Update(id, update);
    }
}