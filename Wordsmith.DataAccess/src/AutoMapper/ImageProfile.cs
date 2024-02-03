using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDto>()
            .ForMember(dest => dest.ImagePath, options => options.MapFrom(image => image.Path));
    }
}