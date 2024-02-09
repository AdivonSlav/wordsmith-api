using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.RequestObjects.EBookReport;

namespace Wordsmith.DataAccess.AutoMapper;

public class EBookReportProfile : Profile
{
    public EBookReportProfile()
    {
        CreateMap<EBookReportInsertRequest, EBookReport>()
            .ForMember(dest => dest.ReportDetails, options =>
            {
                options.MapFrom(src => new ReportDetails()
                {
                    Content = src.Content,
                    ReportReasonId = src.ReportReasonId
                });
            })
            .ForMember(dest => dest.EBookId, options => options.MapFrom(src => src.ReportedEBookId));
        CreateMap<EBookReportUpdateRequest, EBookReport>()
            .ForPath(dest => dest.ReportDetails.IsClosed, options => options.MapFrom(src => src.IsClosed));
        CreateMap<EBookReport, EBookReportDto>();
    }
}