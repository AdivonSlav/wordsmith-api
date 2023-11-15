using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
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

        // Report Reason and Details
        CreateMap<ReportReason, ReportReasonDto>();
        CreateMap<ReportDetails, ReportDetailsDto>();
        
        // User Reports
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
        
        // eBook Reports
        CreateMap<EBookReportInsertRequest, EBookReport>()
            .ForMember(dest => dest.ReportDetails, options =>
            {
                options.MapFrom(src => new ReportDetails()
                {
                    Content = src.Content,
                    ReportReasonId = src.ReportReasonId
                });
            });
        CreateMap<EBookReportUpdateRequest, EBookReport>()
            .ForPath(dest => dest.ReportDetails.IsClosed, options => options.MapFrom(src => src.IsClosed));
        CreateMap<EBookReport, EBookReportDto>();
        
        // eBooks
        CreateMap<EBookInsertRequest, EBook>()
            .ForMember(dest => dest.CoverArt, options => options.Ignore())
            .ForMember(dest => dest.Title, options => options.MapFrom(src => src.ParsedInfo.Title))
            .ForMember(dest => dest.Description, options => options.MapFrom(src => src.ParsedInfo.Description));
        CreateMap<EBook, EBookDto>();
        
        // Genres
        CreateMap<Genre, GenreDto>();
        
        // Maturity Ratings
        CreateMap<MaturityRating, MaturityRatingDto>();
    }
}