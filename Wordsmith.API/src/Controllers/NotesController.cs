using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.Note;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Note;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("notes")]
public class NotesController : WriteController<NoteDto, Note, NoteSearchObject, NoteInsertRequest, NoteUpdateRequest>
{
    public NotesController(INoteService noteService) : base(noteService)
    {
    }
    
    [SwaggerOperation("Add a note")]
    [Authorize("All")]
    public override Task<ActionResult<EntityResult<NoteDto>>> Insert(NoteInsertRequest insert)
    {
        insert.UserId = GetAuthUserId();
        return base.Insert(insert);
    }
    
    [NonAction]
    public override Task<ActionResult<EntityResult<NoteDto>>> Update(int id, NoteUpdateRequest update)
    {
        return base.Update(id, update);
    }
    
    [SwaggerOperation("Delete a note")]
    [Authorize("All")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<EntityResult<NoteDto>>> Delete(int id)
    {
        var userId = GetAuthUserId();
        return Ok(await (WriteService as INoteService)!.Delete(userId, id));
    }
}