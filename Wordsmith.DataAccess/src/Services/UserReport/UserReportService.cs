using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Models.RequestObjects.UserReport;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.DataAccess.Services.UserReport;

public class UserReportService : WriteService<UserReportDto, Db.Entities.UserReport, UserReportSearchObject,
    UserReportInsertRequest, UserReportUpdateRequest>, IUserReportService
{
    private readonly IMessageProducer _messageProducer;

    public UserReportService(DatabaseContext context, IMapper mapper, IMessageProducer messageProducer) : base(context,
        mapper)
    {
        _messageProducer = messageProducer;
    }

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

        if (search.StartDate != null)
        {
            query = query.Where(e => e.ReportDetails.SubmissionDate.Date >= search.StartDate.Value.Date);
        }

        if (search.EndDate != null)
        {
            query = query.Where(e => e.ReportDetails.SubmissionDate.Date <= search.EndDate.Value.Date);
        }

        return query;
    }

    protected override async Task BeforeInsert(Db.Entities.UserReport entity, UserReportInsertRequest insert,
        int userId)
    {
        await ValidateInsertion(insert);

        entity.ReportDetails.UserId = insert.ReporterUserId;
        await Context.Entry(entity).Reference(e => e.ReportedUser).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.Reporter).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.ReportReason).LoadAsync();

        entity.ReportDetails.SubmissionDate = DateTime.UtcNow;
        entity.ReportDetails.IsClosed = false;
    }

    protected override async Task BeforeUpdate(Db.Entities.UserReport entity, UserReportUpdateRequest update,
        int userId)
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

    public async Task<EntityResult<UserReportDto>> SendEmail(UserReportEmailSendRequest request)
    {
        await ValidateSendEmail(request);

        var report = await Context.UserReports
            .Include(e => e.ReportedUser)
            .Include(e => e.ReportDetails)
            .ThenInclude(reportDetails => reportDetails.ReportReason)
            .Include(e => e.ReportDetails.Reporter)
            .SingleAsync(e => e.Id == request.ReportId);

        request.Body += $"<br><br>Report ID: {report.Id}<br>Report reason: {report.ReportDetails.ReportReason.Reason}";
        
        var emailMessage = new SendEmailMessage()
        {
            EmailToId = report.ReportedUser.Email,
            EmailToName = report.ReportedUser.Username,
            EmailSubject = $"Report filed against you",
            EmailBody = request.Body,
        };

        _messageProducer.SendMessage("send_email", emailMessage);

        return new EntityResult<UserReportDto>()
        {
            Message = "Successfully sent email",
            Result = Mapper.Map<UserReportDto>(report)
        };
    }

    private async Task ValidateSendEmail(UserReportEmailSendRequest request)
    {
        var report = await Context.UserReports
            .Include(e => e.ReportDetails)
            .FirstOrDefaultAsync(e => e.Id == request.ReportId);

        if (report == null)
        {
            throw new AppException("The requested report does not exist!");
        }

        if (report.ReportDetails.IsClosed)
        {
            throw new AppException("You cannot send an email for a closed report!");
        }
    }
}