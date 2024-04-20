using Microsoft.AspNetCore.Mvc;
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
}