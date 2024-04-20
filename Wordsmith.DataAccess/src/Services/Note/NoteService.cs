using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Note;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Note;

public class NoteService : WriteService<NoteDto, Db.Entities.Note, NoteSearchObject, NoteInsertRequest, NoteUpdateRequest>, INoteService
{
    public NoteService(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {
    }
}