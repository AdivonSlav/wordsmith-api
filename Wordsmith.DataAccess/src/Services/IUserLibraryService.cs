using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IUserLibraryService : IWriteService<UserLibraryDto, UserLibrary, UserLibrarySearchObject, UserLibraryInsertRequest, UserLibraryUpdateRequest>
{
    // public Task<ActionResult<QueryResult<UserLibraryDto>>> GetLibraryEntry(int userId, int eBookId);
}