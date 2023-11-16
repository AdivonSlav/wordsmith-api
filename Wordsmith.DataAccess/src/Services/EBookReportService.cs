using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class EBookReportService : WriteService<EBookReportDto, EBookReport, EBookReportSearchObject, EBookReportInsertRequest, EBookReportUpdateRequest>, IEBookReportService
{
    public EBookReportService(DatabaseContext context, IMapper mapper)
        : base(context, mapper) {}

    protected override IQueryable<EBookReport> AddInclude(IQueryable<EBookReport> query, EBookReportSearchObject? search = null)
    {
        query = query.Include(report => report.ReportedEBook)
            .ThenInclude(eBook => eBook.CoverArt)
            .Include(report => report.ReportedEBook)
            .ThenInclude(eBook => eBook.Genre)
            .Include(report => report.ReportedEBook)
            .ThenInclude(eBook => eBook.MaturityRating)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.Reporter)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.ReportReason);

        return query;
    }

    protected override IQueryable<EBookReport> AddFilter(IQueryable<EBookReport> query, EBookReportSearchObject? search = null)
    {
        if (search?.ReportedEBookId != null)
        {
            query = query.Where(report => report.EBookId == search.ReportedEBookId.Value);
        }

        if (search?.IsClosed != null)
        {
            query = query.Where(report => report.ReportDetails.IsClosed == search.IsClosed.Value);
        }

        return query;
    }

    protected override async Task BeforeInsert(EBookReport entity, EBookReportInsertRequest insert)
    {
        if (!insert.ReporterUserId.HasValue) throw new AppException("The user making the report does not exist!");

        entity.ReportDetails.UserId = insert.ReporterUserId.Value;
        await Context.Entry(entity).Reference(e => e.ReportedEBook).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.Reporter).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.ReportReason).LoadAsync();

        if (entity.ReportedEBook == null) throw new AppException("The reported eBook does not exist");

        entity.ReportDetails.SubmissionDate = DateTime.UtcNow;
        entity.ReportDetails.IsClosed = false;
    }

    protected override async Task BeforeUpdate(EBookReport entity, EBookReportUpdateRequest update)
    {
        await Context.Entry(entity).Reference(e => e.ReportDetails).LoadAsync();
    }
}