using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBookRating;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookRating;

public interface IEBookRatingService : IWriteService<EBookRatingDto, Db.Entities.EBookRating, EBookRatingSearchObject, EBookRatingInsertRequest, EBookRatingUpdateRequest>
{
    Task<QueryResult<EBookRatingStatisticsDto>> GetStatistics(int eBookId);
}