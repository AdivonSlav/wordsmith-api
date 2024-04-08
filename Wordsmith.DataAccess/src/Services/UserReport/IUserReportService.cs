using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.UserReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserReport;

public interface IUserReportService : IWriteService<UserReportDto, Db.Entities.UserReport, UserReportSearchObject,
    UserReportInsertRequest, UserReportUpdateRequest>
{
    Task<EntityResult<UserReportDto>> SendEmail(UserReportEmailSendRequest request);
}