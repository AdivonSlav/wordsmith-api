using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models;
using Wordsmith.Models.RequestObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Users
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

        // Images
        CreateMap<Image, ImageDto>()
            .ForMember(dest => dest.ImagePath, options => options.MapFrom(image => image.Path));

        // User Reports
        CreateMap<ReportReason, ReportReasonDto>();
        CreateMap<ReportDetails, ReportDetailsDto>();
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