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
            .ForMember(dest => dest.ProfileImage, options => options.Ignore())
            .ForAllMembers(options =>
            {
                // Only map from the update object if the source member is not null.
                options.Condition((src, dest, sourceMember) => sourceMember != null);
            });
        
        CreateMap<Image, ImageDto>()
            .ForMember(dest => dest.ImagePath, options => options.MapFrom(image => image.Path));
    }
}