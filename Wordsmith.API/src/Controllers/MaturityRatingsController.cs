using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
using Wordsmith.DataAccess.Services.MaturityRating;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("maturity-ratings")]
public class MaturityRatingsController : ReadController<MaturityRatingDto, MaturityRatingSearchObject>
{
    public MaturityRatingsController(IMaturityRatingService maturityRatingService) : base(maturityRatingService) { }
    
    
}