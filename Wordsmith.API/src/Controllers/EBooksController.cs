using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.DataAccess.Services.EBook;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Models.ValidationAttributes;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("ebooks")]
public class
    EBooksController : WriteController<EBookDto, EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>
{
    public EBooksController(IEBookService eBookService) : base(eBookService) { }

    [Authorize("All")]
    public override Task<ActionResult<EBookDto>> Insert([FromForm] EBookInsertRequest insert)
    {
        return base.Insert(insert);
    }

    [Authorize("All")]
    [HttpPost("parse")]
    public async Task<ActionResult<EBookParseDto>> Parse([EpubFile] IFormFile file)
    {
        return await (WriteService as IEBookService)!.Parse(file);
    }

    [Authorize("All")]
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        return await (WriteService as IEBookService)!.Download(id);
    }
}