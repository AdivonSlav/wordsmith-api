using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.EBook;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBook;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Models.ValidationAttributes;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("ebooks")]
public class
    EBooksController : WriteController<EBookDto, EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>
{
    public EBooksController(IEBookService eBookService) : base(eBookService) { }

    [SwaggerOperation("Adds a new ebook")]
    [Authorize("All")]
    public override Task<ActionResult<EntityResult<EBookDto>>> Insert([FromForm] EBookInsertRequest insert)
    {
        return base.Insert(insert);
    }

    [SwaggerOperation("Parses an EPUB file and returns metadata information")]
    [Authorize("All")]
    [HttpPost("parse")]
    public async Task<ActionResult<EntityResult<EBookDto>>> Parse([EpubFile] IFormFile file)
    {
        return Ok(await (WriteService as IEBookService)!.Parse(file));
    }

    [SwaggerOperation("Downloads the EPUB file of an ebook")]
    [Authorize("All")]
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var ebookFile = await (WriteService as IEBookService)!.Download(id);

        return File(ebookFile.Bytes, ebookFile.MimeType, fileDownloadName: ebookFile.Filename);
    }
}