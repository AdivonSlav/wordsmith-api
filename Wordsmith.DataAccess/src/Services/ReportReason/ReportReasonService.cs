using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.ReportReason;

public class ReportReasonService : ReadService<ReportReasonDto, Db.Entities.ReportReason, ReportReasonSearch>, IReportReasonService
{
    public ReportReasonService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override IQueryable<Db.Entities.ReportReason> AddFilter(IQueryable<Db.Entities.ReportReason> query,
        ReportReasonSearch search, int userId)
    {
        if (search.Type.HasValue)
        {
            return query.Where(reason => reason.Type == search.Type);
        }

        return query;
    }
}