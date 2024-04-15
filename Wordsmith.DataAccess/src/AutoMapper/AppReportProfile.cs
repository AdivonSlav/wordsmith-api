using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.AppReport;

namespace Wordsmith.DataAccess.AutoMapper;

public class AppReportProfile : Profile
{
    public AppReportProfile()
    {
        CreateMap<AppReportInsertRequest, AppReport>();
        CreateMap<AppReportUpdateRequest, AppReport>();
        CreateMap<AppReport, AppReportDto>();
    }
}