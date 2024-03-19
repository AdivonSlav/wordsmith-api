using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBook;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookRating;

public interface IEBookRatingService : IWriteService<EBookRatingDto, Db.Entities.EBookRating, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>
{
    
}