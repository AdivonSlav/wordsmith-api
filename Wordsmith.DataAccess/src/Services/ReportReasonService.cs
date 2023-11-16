using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class ReportReasonService : ReadService<ReportReasonDto, ReportReason, ReportReasonSearch>, IReportReasonService
{
    public ReportReasonService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override IQueryable<ReportReason> AddFilter(IQueryable<ReportReason> query, ReportReasonSearch? search = null)
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