using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("maturity-ratings")]
public class MaturityRatingController : ReadController<MaturityRatingDto, MaturityRatingSearchObject>
{
    public MaturityRatingController(IMaturityRatingService maturityRatingService) : base(maturityRatingService) { }
    
    
}