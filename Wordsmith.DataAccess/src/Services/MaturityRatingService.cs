using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class MaturityRatingService : ReadService<MaturityRatingDto, MaturityRating, MaturityRatingSearchObject>, IMaturityRatingService
{
    public MaturityRatingService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}