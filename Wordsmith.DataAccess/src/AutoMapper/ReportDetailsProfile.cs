using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class ReportDetailsProfile : Profile
{
    public ReportDetailsProfile()
    {
        CreateMap<ReportDetails, ReportDetailsDto>();
    }
}