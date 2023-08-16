
using AutoMapper;
using Wordsmith.DataAccess.DB.Entities;
using Wordsmith.Models;

namespace Wordsmith.DataAccess.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReportReason, ReportReasonDTO>();
        }
    }
}
