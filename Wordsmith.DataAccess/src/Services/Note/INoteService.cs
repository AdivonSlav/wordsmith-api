using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Note;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Note;

public interface INoteService : IWriteService<NoteDto, Db.Entities.Note, NoteSearchObject, NoteInsertRequest, NoteUpdateRequest>
{
    
}