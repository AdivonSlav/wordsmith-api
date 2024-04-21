using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.Note;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Note;

public class NoteService : WriteService<NoteDto, Db.Entities.Note, NoteSearchObject, NoteInsertRequest, NoteUpdateRequest>, INoteService
{
    public NoteService(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {
    }
    
    protected override IQueryable<Db.Entities.Note> AddFilter(IQueryable<Db.Entities.Note> query, NoteSearchObject search, int userId)
    {
        if (search.UserId.HasValue)
        {
            query = query.Where(e => e.UserId == search.UserId);
        }
        
        if (search.EBookId.HasValue)
        {
            query = query.Where(e => e.EBookId == search.EBookId);
        }
        
        return query;
    }
    
    protected override async Task BeforeInsert(Db.Entities.Note entity, NoteInsertRequest insert, int userId)
    {
        await ValidateInsertion(insert);
        entity.DateAdded = DateTime.UtcNow;
    }
    
    private async Task ValidateInsertion(NoteInsertRequest request)
    {
        if (!await Context.EBooks.AnyAsync(e => e.Id == request.EBookId))
        {
            throw new AppException("Ebook does not exist!");
        }
        
        if (!await Context.Users.AnyAsync(e => e.Id == request.UserId))
        {
            throw new AppException("User does not exist!");
        }
    }
}