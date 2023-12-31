using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class UserReportService : WriteService<UserReportDto, UserReport, UserReportSearchObject,
    UserReportInsertRequest, UserReportUpdateRequest>, IUserReportService
{
    public UserReportService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override IQueryable<UserReport> AddInclude(IQueryable<UserReport> query,
        UserReportSearchObject? search = null)
    {
        query = query.Include(report => report.ReportedUser)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.Reporter)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.ReportReason);

        return query;
    }

    protected override IQueryable<UserReport> AddFilter(IQueryable<UserReport> query,
        UserReportSearchObject? search = null)
    {
        if (search?.ReportedUserId != null)
        {
            query = query.Where(report => report.ReportedUserId == search.ReportedUserId.Value);
        }

        if (search?.IsClosed != null)
        {
            query = query.Where(report => report.ReportDetails.IsClosed == search.IsClosed.Value);
        }

        if (search?.Reason != null)
        {
            query = query.Where(report => report.ReportDetails.ReportReason.Reason == search.Reason);
        }

        if (search?.ReportDate != null)
        {
            query = query.Where(report => report.ReportDetails.SubmissionDate.Day == search.ReportDate.Value.Day &&
                                          report.ReportDetails.SubmissionDate.Month == search.ReportDate.Value.Month &&
                                          report.ReportDetails.SubmissionDate.Year == search.ReportDate.Value.Year);
        }

        return query;
    }

    protected override async Task BeforeInsert(UserReport entity, UserReportInsertRequest insert)
    {
        if (!insert.ReporterUserId.HasValue) throw new AppException("The user making the report does not exist!");

        if (insert.ReporterUserId.Value == insert.ReportedUserId)
        {
            throw new AppException("You cannot report yourself!");
        }

        entity.ReportDetails.UserId = insert.ReporterUserId.Value;
        await Context.Entry(entity).Reference(e => e.ReportedUser).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.Reporter).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.ReportReason).LoadAsync();

        if (entity.ReportedUser == null) throw new AppException("The reported user does not exist");

        entity.ReportDetails.SubmissionDate = DateTime.UtcNow;
        entity.ReportDetails.IsClosed = false;
    }

    protected override async Task BeforeUpdate(UserReport entity, UserReportUpdateRequest update)
    {
        await Context.Entry(entity).Reference(e => e.ReportDetails).LoadAsync();
    }
}