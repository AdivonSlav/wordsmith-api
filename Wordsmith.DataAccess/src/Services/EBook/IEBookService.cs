using Microsoft.AspNetCore.Http;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBook;
using Wordsmith.Models.RequestObjects.Statistics;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils.EBookFileHelper;

namespace Wordsmith.DataAccess.Services.EBook;

public interface IEBookService : IWriteService<EBookDto, Db.Entities.EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>
{
    public Task<EntityResult<EBookParseDto>> Parse(IFormFile file);
    
    public Task<EBookFile> Download(int id);
    
    public Task<EntityResult<EBookDto>> Hide(int id);
    
    public Task<EntityResult<EBookDto>> Unhide(int id);
    
    public Task<QueryResult<EBookPublishStatisticsDto>> GetPublishStatistics(StatisticsRequest request);
    
    public Task<QueryResult<EBookTrafficStatisticsDto>> GetTrafficStatistics(StatisticsRequest request);
}
