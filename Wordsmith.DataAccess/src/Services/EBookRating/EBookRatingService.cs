using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBookRating;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookRating;

public class EBookRatingService : WriteService<EBookRatingDto, Db.Entities.EBookRating, EBookRatingSearchObject,
    EBookRatingInsertRequest, EBookRatingUpdateRequest>, IEBookRatingService
{
    public EBookRatingService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}