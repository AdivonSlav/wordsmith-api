using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.RequestObjects.UserLibrary;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserLibrary;

public interface IUserLibraryService : IWriteService<UserLibraryDto, Db.Entities.UserLibrary, UserLibrarySearchObject, UserLibraryInsertRequest, UserLibraryUpdateRequest>
{
    // public Task<ActionResult<QueryResult<UserLibraryDto>>> GetLibraryEntry(int userId, int eBookId);
}