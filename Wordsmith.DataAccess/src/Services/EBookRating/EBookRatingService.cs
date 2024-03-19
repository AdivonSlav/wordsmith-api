using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBook;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookRating;

public class EBookRatingService : WriteService<EBookRatingDto, Db.Entities.EBookRating, EBookSearchObject,
    EBookInsertRequest, EBookUpdateRequest>, IEBookRatingService
{
    public EBookRatingService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}