using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.ReportReason;

public class ReportReasonService : ReadService<ReportReasonDto, Db.Entities.ReportReason, ReportReasonSearch>, IReportReasonService
{
    public ReportReasonService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override IQueryable<Db.Entities.ReportReason> AddFilter(IQueryable<Db.Entities.ReportReason> query, ReportReasonSearch? search = null)
    {
        if (!string.IsNullOrEmpty(search?.Subject))
        {
            // MySQL string comparison is case-insensitive by default, so this will allow even stuff like (uSeR == user) to pass
            // May be worth to force a case-sensitive comparison
            return query.Where(reason => reason.Subject == search.Subject);
        }

        return query;
    }
}