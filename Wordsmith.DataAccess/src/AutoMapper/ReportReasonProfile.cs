using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class ReportReasonProfile : Profile
{
    public ReportReasonProfile()
    {
        CreateMap<ReportReason, ReportReasonDto>();
    }
}