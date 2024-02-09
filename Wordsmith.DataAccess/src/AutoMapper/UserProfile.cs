using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.RequestObjects.User;

namespace Wordsmith.DataAccess.AutoMapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
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
    }
}