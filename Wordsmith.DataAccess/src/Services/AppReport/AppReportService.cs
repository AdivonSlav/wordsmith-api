using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.AppReport;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.AppReport;

public class AppReportService : WriteService<AppReportDto, Db.Entities.AppReport, AppReportSearchObject, AppReportInsertRequest, AppReportUpdateRequest>, IAppReportService
{
    public AppReportService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override IQueryable<Db.Entities.AppReport> AddInclude(IQueryable<Db.Entities.AppReport> query, int userId)
    {
        return query.Include(e => e.User);
    }

    protected override IQueryable<Db.Entities.AppReport> AddFilter(IQueryable<Db.Entities.AppReport> query, AppReportSearchObject search, int userId)
    {
        if (search.UserId.HasValue)
        {
            query = query.Where(e => e.UserId == search.UserId.Value);
        }

        if (search.IsClosed.HasValue)
        {
            query = query.Where(e => e.IsClosed == search.IsClosed.Value);
        }
        
        if (search.StartDate != null)
        {
            query = query.Where(e => e.SubmissionDate.Date >= search.StartDate.Value.Date);
        }

        if (search.EndDate != null)
        {
            query = query.Where(e => e.SubmissionDate.Date <= search.EndDate.Value.Date);
        }

        return query;
    }

    protected override async Task BeforeInsert(Db.Entities.AppReport entity, AppReportInsertRequest insert, int userId)
    {
        await ValidateInsertion(insert);

        await Context.Entry(entity).Reference(e => e.User).LoadAsync();
        entity.SubmissionDate = DateTime.UtcNow;
    }

    protected override async Task BeforeUpdate(Db.Entities.AppReport entity, AppReportUpdateRequest update, int userId)
    {
        await Context.Entry(entity).Reference(e => e.User).LoadAsync();
    }
    
    private async Task ValidateInsertion(AppReportInsertRequest insert)
    {
        if (!await Context.Users.AnyAsync(e => e.Id == insert.UserId))
        {
            throw new AppException("The user making the report does not exist!");
        }
    }
}