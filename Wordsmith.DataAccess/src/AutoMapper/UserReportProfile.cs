using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class UserReportProfile : Profile
{
    public UserReportProfile()
    {
        CreateMap<UserReportInsertRequest, UserReport>()
            .ForMember(dest => dest.ReportDetails, options =>
            {
                options.MapFrom(src => new ReportDetails()
                {
                    Content = src.Content,
                    ReportReasonId = src.ReportReasonId
                });
            });
        CreateMap<UserReportUpdateRequest, UserReport>()
            .ForPath(dest => dest.ReportDetails.IsClosed, options => options.MapFrom(src => src.IsClosed));
        CreateMap<UserReport, UserReportDto>();
    }
}