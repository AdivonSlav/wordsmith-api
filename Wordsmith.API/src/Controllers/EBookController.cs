using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Models.ValidationAttributes;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("ebooks")]
public class
    EBookController : WriteController<EBookDto, EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>
{
    public EBookController(IEBookService eBookService) : base(eBookService) { }

    public override Task<ActionResult<EBookDto>> Insert([FromForm] EBookInsertRequest insert)
    {
        return base.Insert(insert);
    }

    [HttpPost("parse")]
    public async Task<ActionResult<EBookParseDto>> Parse([EpubFile] IFormFile file)
    {
        return await (WriteService as IEBookService)!.Parse(file);
    }
}