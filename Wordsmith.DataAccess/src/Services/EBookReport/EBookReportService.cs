using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Models.RequestObjects.EBookReport;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.DataAccess.Services.EBookReport;

public class EBookReportService : WriteService<EBookReportDto, Db.Entities.EBookReport, EBookReportSearchObject, EBookReportInsertRequest, EBookReportUpdateRequest>, IEBookReportService
{
    private readonly IMessageProducer _messageProducer;

    public EBookReportService(DatabaseContext context, IMapper mapper, IMessageProducer messageProducer)
        : base(context, mapper)
    {
        _messageProducer = messageProducer;
    }

    protected override IQueryable<Db.Entities.EBookReport> AddInclude(IQueryable<Db.Entities.EBookReport> query, int userId)
    {
        query = query.Include(report => report.ReportedEBook)
            .ThenInclude(eBook => eBook.CoverArt)
            .Include(report => report.ReportedEBook)
            .ThenInclude(eBook => eBook.MaturityRating)
            .Include(eBook => eBook.ReportedEBook)
            .ThenInclude(eBook => eBook.Author)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.Reporter)
            .Include(report => report.ReportDetails)
            .ThenInclude(details => details.ReportReason);
    
        return query;
    }

    protected override IQueryable<Db.Entities.EBookReport> AddFilter(IQueryable<Db.Entities.EBookReport> query,
        EBookReportSearchObject search, int userId)
    {
        if (search.ReportedEBookId != null)
        {
            query = query.Where(report => report.EBookId == search.ReportedEBookId.Value);
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

    protected override async Task BeforeInsert(Db.Entities.EBookReport entity, EBookReportInsertRequest insert,
        int userId)
    {
        await ValidateInsertion(insert);
        
        entity.ReportDetails.UserId = insert.ReporterUserId;
        await Context.Entry(entity).Reference(e => e.ReportedEBook).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.Reporter).LoadAsync();
        await Context.Entry(entity.ReportDetails).Reference(e => e.ReportReason).LoadAsync();
        
        entity.ReportDetails.SubmissionDate = DateTime.UtcNow;
        entity.ReportDetails.IsClosed = false;
    }

    protected override async Task BeforeUpdate(Db.Entities.EBookReport entity, EBookReportUpdateRequest update,
        int userId)
    {
        await Context.Entry(entity).Reference(e => e.ReportDetails).LoadAsync();
    }
    
    private async Task ValidateInsertion(EBookReportInsertRequest insert)
    {
        var ebook = await Context.EBooks.FindAsync(insert.ReportedEBookId);

        if (ebook != null)
        {
            if (!await Context.Users.AnyAsync(e => e.Id == insert.ReporterUserId))
            {
                throw new AppException("The user making the report does not exist!");
            }

            if (insert.ReporterUserId == ebook.AuthorId)
            {
                throw new AppException("You cannot report an ebook you own!");
            }
        }
        else
            throw new AppException("The reported eBook does not exist");
    }

    public async Task<EntityResult<EBookReportDto>> SendEmail(EBookReportEmailSendRequest request)
    {
        await ValidateSendEmail(request);

        var report = await Context.EBookReports
            .Include(e => e.ReportedEBook)
            .ThenInclude(eBook => eBook.Author)
            .Include(e => e.ReportedEBook)
            .ThenInclude(e => e.MaturityRating)
            .Include(e => e.ReportedEBook)
            .ThenInclude(e => e.CoverArt)
            .Include(e => e.ReportDetails)
            .ThenInclude(e => e.ReportReason)
            .Include(e => e.ReportDetails.Reporter)
            .SingleAsync(e => e.Id == request.ReportId);
        
        request.Body += @$"<br><br>Report ID: {report.Id}<br>Report reason: {report.ReportDetails.ReportReason.Reason}
          <br>Reported ebook: {report.ReportedEBook.Title}";
        
        var emailMessage = new SendEmailMessage()
        {
            EmailToId = report.ReportedEBook.Author.Email,
            EmailToName = report.ReportedEBook.Author.Username,
            EmailSubject = $"Report filed against an ebook you authored",
            EmailBody = request.Body,
        };
        
        _messageProducer.SendMessage("send_email", emailMessage);

        return new EntityResult<EBookReportDto>()
        {
            Message = "Successfully sent email",
            Result = Mapper.Map<EBookReportDto>(report)
        };
    }
    
    private async Task ValidateSendEmail(EBookReportEmailSendRequest request)
    {
        var report = await Context.EBookReports
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