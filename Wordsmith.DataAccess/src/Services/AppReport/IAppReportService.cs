using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.AppReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.AppReport;

public interface IAppReportService : IWriteService<AppReportDto, Db.Entities.AppReport, AppReportSearchObject, AppReportInsertRequest, AppReportUpdateRequest>
{
    
}