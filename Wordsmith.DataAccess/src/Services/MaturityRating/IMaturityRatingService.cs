using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.MaturityRating;

public interface IMaturityRatingService : IReadService<MaturityRatingDto, MaturityRatingSearchObject>
{
    
}