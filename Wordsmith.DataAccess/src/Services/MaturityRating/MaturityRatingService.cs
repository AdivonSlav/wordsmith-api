using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.MaturityRating;

public class MaturityRatingService : ReadService<MaturityRatingDto, Db.Entities.MaturityRating, MaturityRatingSearchObject>, IMaturityRatingService
{
    public MaturityRatingService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}