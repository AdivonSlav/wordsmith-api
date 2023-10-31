using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IEBookReportService : IWriteService<EBookReportDto, EBookReport, EBookReportSearchObject, EBookReportInsertRequest, EBookReportUpdateRequest>
{
    
}