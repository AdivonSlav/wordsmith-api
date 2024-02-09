using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.RequestObjects.EBook;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBook;

public interface IEBookService : IWriteService<EBookDto, Db.Entities.EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>
{
    public Task<ActionResult<EBookParseDto>> Parse(IFormFile file);
    public Task<IActionResult> Download(int id);
}
