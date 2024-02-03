using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class EBookProfile : Profile
{
    public EBookProfile()
    {
        CreateMap<EBookInsertRequest, EBook>()
            .ForMember(dest => dest.CoverArt, options => options.Ignore());
        CreateMap<EBook, EBookDto>();
    }
}