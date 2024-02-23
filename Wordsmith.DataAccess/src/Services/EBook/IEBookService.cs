using Microsoft.AspNetCore.Http;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBook;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils.EBookFileHelper;

namespace Wordsmith.DataAccess.Services.EBook;

public interface IEBookService : IWriteService<EBookDto, Db.Entities.EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>
{
    public Task<EntityResult<EBookParseDto>> Parse(IFormFile file);
    public Task<EBookFile> Download(int id);
}
