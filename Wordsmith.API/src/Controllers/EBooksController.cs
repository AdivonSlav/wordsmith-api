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

    [SwaggerOperation("Update an existing ebook")]
    [Authorize("All")]
    public override Task<ActionResult<EntityResult<EBookDto>>> Update(int id, EBookUpdateRequest update)
    {
        return base.Update(id, update);
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

    [SwaggerOperation("Hide an ebook from being displayed")]
    [HttpPut("{id:int}/hide")]
    [Authorize("AdminOperations")]
    public async Task<ActionResult<EntityResult<EBookDto>>> Hide(int id)
    {
        return Ok(await (WriteService as IEBookService)!.Hide(id));
    }
    
    [SwaggerOperation("Reveal a hidden ebook for displaying")]
    [HttpPut("{id:int}/unhide")]
    [Authorize("AdminOperations")]
    public async Task<ActionResult<EntityResult<EBookDto>>> Unhide(int id)
    {
        return Ok(await (WriteService as IEBookService)!.Unhide(id));
    }
}