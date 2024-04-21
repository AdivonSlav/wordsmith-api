using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Note;

namespace Wordsmith.DataAccess.AutoMapper;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<Note, NoteDto>();
        CreateMap<NoteInsertRequest, Note>();
        CreateMap<NoteUpdateRequest, Note>();
    }
}