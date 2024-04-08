using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBookReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookReport;

public interface IEBookReportService : IWriteService<EBookReportDto, Db.Entities.EBookReport, EBookReportSearchObject, EBookReportInsertRequest, EBookReportUpdateRequest>
{
    Task<EntityResult<EBookReportDto>> SendEmail(EBookReportEmailSendRequest request);
}