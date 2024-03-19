using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.EBookRating;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBookRating;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("ebook-ratings")]
public class EBookRatingsController : WriteController<EBookRatingDto, EBookRating, EBookRatingSearchObject, EBookRatingInsertRequest, EBookRatingUpdateRequest>
{
    public EBookRatingsController(IEBookRatingService eBookRatingService) : base(eBookRatingService) { }

    [SwaggerOperation("Insert a new ebook rating")]
    [Authorize("All")]
    public override Task<ActionResult<EntityResult<EBookRatingDto>>> Insert(EBookRatingInsertRequest insert)
    {
        insert.UserId = GetAuthUserId();
        return base.Insert(insert);
    }

    [NonAction]
    public override Task<ActionResult<EntityResult<EBookRatingDto>>> Update(int id, EBookRatingUpdateRequest update)
    {
        return base.Update(id, update);
    }
}