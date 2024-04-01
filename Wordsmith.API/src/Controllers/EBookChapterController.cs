using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services.EBookChapter;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("ebook-chapters")]
public class EBookChapterController : ReadController<EBookChapterDto, EBookChapterSearchObject>
{
    public EBookChapterController(IEBookChapterService chapterService) : base(chapterService) { }
}