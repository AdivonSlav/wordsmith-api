using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.UserReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserReport;

public class UserReportService : WriteService<UserReportDto, Db.Entities.UserReport, UserReportSearchObject,
    UserReportInsertRequest, UserReportUpdateRequest>, IUserReportService
{
    public UserReportService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override IQueryable<Db.Entities.UserReport> AddInclude(IQueryable<Db.Entities.UserReport> query,
        int userId)
    {
        query = query.Include(report => report.ReportedUser)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.Reporter)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.ReportReason);
    
        return query;
    }

    protected override IQueryable<Db.Entities.UserReport> AddFilter(IQueryable<Db.Entities.UserReport> query,
        UserReportSearchObject search, int userId)
    {
        if (search.ReportedUserId != null)
        {
            query = query.Where(report => report.ReportedUserId == search.ReportedUserId.Value);
        }

        if (search.IsClosed != null)
        {
            query = query.Where(report => report.ReportDetails.IsClosed == search.IsClosed.Value);
        }

        if (search.Reason != null)
        {
            query = query
                .Where(report => report.ReportDetails.ReportReason.Reason
                    .Contains(search.Reason, StringComparison.OrdinalIgnoreCase));
        }

        if (search.ReportDate != null)
        {
            query = query.Where(report => report.ReportDetails.SubmissionDate.Day == search.ReportDate.Value.Day &&
                                          report.ReportDetails.SubmissionDate.Month == search.ReportDate.Value.Month &&
                                          report.ReportDetails.SubmissionDate.Year == search.ReportDate.Value.Year);
        }

        return query;
    }

    protected override async Task BeforeInsert(Db.Entities.UserReport entity, UserReportInsertRequest insert)
    {
        await ValidateInsertion(insert);

        entity.ReportDetails.UserId = insert.ReporterUserId;
        await Context.Entry(entity).Reference(e => e.ReportedUser).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.Reporter).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.ReportReason).LoadAsync();

        entity.ReportDetails.SubmissionDate = DateTime.UtcNow;
        entity.ReportDetails.IsClosed = false;
    }

    protected override async Task BeforeUpdate(Db.Entities.UserReport entity, UserReportUpdateRequest update)
    {
        await Context.Entry(entity).Reference(e => e.ReportDetails).LoadAsync();
    }

    private async Task ValidateInsertion(UserReportInsertRequest insert)
    {
        if (insert.ReporterUserId == insert.ReportedUserId)
        {
            throw new AppException("You cannot report yourself!");
        }

        if (!await Context.Users.AnyAsync(e => e.Id == insert.ReporterUserId))
        {
            throw new AppException("The user making the report does not exist!");
        }

        if (!await Context.Users.AnyAsync(e => e.Id == insert.ReportedUserId))
        {
            throw new AppException("The user you are trying to report does not exist!");
        }
    }
}