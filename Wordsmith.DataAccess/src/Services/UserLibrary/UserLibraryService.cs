using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.RequestObjects.UserLibrary;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserLibrary;

public class UserLibraryService : WriteService<UserLibraryDto, Db.Entities.UserLibrary, UserLibrarySearchObject, UserLibraryInsertRequest, UserLibraryUpdateRequest>, IUserLibraryService
{
    public UserLibraryService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override async Task BeforeInsert(Db.Entities.UserLibrary entity, UserLibraryInsertRequest insert)
    {
        await ValidateInsertion(entity, insert);
        
        entity.IsRead = false;
        entity.SyncDate = DateTime.UtcNow;
        entity.LastPage = 0;
    }

    protected override async Task AfterInsert(Db.Entities.UserLibrary entity, UserLibraryInsertRequest insert)
    {
        await Context.Entry(entity).Reference(e => e.EBook).LoadAsync();
        await Context.Entry(entity.EBook).Reference(e => e.Author).LoadAsync();
        await Context.Entry(entity.EBook).Reference(e => e.CoverArt).LoadAsync();
        await Context.Entry(entity.EBook).Reference(e => e.MaturityRating).LoadAsync();
    }

    private async Task ValidateInsertion(Db.Entities.UserLibrary entity, UserLibraryInsertRequest insert)
    {
        var ebook = await Context.EBooks.FindAsync(insert.EBookId);
        
        if (ebook == null)
        {
            throw new AppException("The ebook does not exist");
        }

        var alreadyAdded = await Context.UserLibraries.AnyAsync(e => e.UserId == insert.UserId && e.EBookId == insert.EBookId);

        if (alreadyAdded)
        {
            throw new AppException("Book is already added to your library");
        }
    }

    protected override IQueryable<Db.Entities.UserLibrary> AddInclude(IQueryable<Db.Entities.UserLibrary> query, UserLibrarySearchObject search)
    {
        query = query
            .Include(e => e.EBook)
            .ThenInclude(b => b.Author)
            .Include(e => e.EBook)
            .ThenInclude(b => b.CoverArt)
            .Include(e => e.EBook)
            .ThenInclude(b => b.MaturityRating);

        return query;
    }

    protected override IQueryable<Db.Entities.UserLibrary> AddFilter(IQueryable<Db.Entities.UserLibrary> query, UserLibrarySearchObject search)
    {
        query = query.Where(e => e.UserId == search.UserId);

        if (search.IsRead.HasValue)
        {
            query = query.Where(e => e.IsRead == search.IsRead.Value);
        }
        
        if (search.MaturityRatingId.HasValue)
        {
            query = query.Where(e => e.EBook.MaturityRatingId == search.MaturityRatingId.Value);
        }

        if (search.EBookId.HasValue)
        {
            query = query.Where(e => e.EBookId == search.EBookId.Value);
        }

        return query;
    }

    // public async Task<ActionResult<QueryResult<UserLibraryDto>>> GetLibraryEntry(int userId, int eBookId)
    // {
    //     var userLibrary = await Context.UserLibraries.Where(e => e.UserId == userId && e.EBookId == eBookId).ToListAsync();
    //     var queryResult = new QueryResult<UserLibraryDto>
    //     {
    //         Result = new List<UserLibraryDto>()
    //     };
    //
    //     if (userLibrary != null)
    //     {
    //         queryResult.Result.Add(Mapper.Map<UserLibraryDto>(userLibrary));
    //     }
    //
    //     return queryResult;
    // }
}