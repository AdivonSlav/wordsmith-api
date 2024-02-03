using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class UserLibraryService : WriteService<UserLibraryDto, UserLibrary, UserLibrarySearchObject, UserLibraryInsertRequest, UserLibraryUpdateRequest>, IUserLibraryService
{
    public UserLibraryService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override async Task BeforeInsert(UserLibrary entity, UserLibraryInsertRequest insert)
    {
        if (await Context.EBooks.FindAsync(insert.EBookId) == null)
        {
            throw new AppException("The ebook does not exist");
        }
        
        entity.IsRead = false;
        entity.SyncDate = DateTime.UtcNow;
        entity.LastPage = 0;
    }
}