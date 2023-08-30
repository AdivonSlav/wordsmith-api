using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models;
using Wordsmith.Models.RequestObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ReportReason, ReportReasonDto>();

        CreateMap<User, UserDto>();
        CreateMap<UserInsertRequest, User>()
            .ForMember(dest => dest.ProfileImage, options => options.Ignore());
        CreateMap<UserUpdateRequest, User>()
            .ForMember(dest => dest.ProfileImage, options => options.Ignore());
        
        CreateMap<Image, ImageDto>()
            .ForMember(dest => dest.ImagePath, options => options.MapFrom(image => image.Path));
    }
}