using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.EBookRating;

namespace Wordsmith.DataAccess.AutoMapper;

public class EBookRatingProfile : Profile
{
    public EBookRatingProfile()
    {
        CreateMap<EBookRatingInsertRequest, EBookRating>();
        CreateMap<EBookRatingUpdateRequest, EBookRating>();
        CreateMap<EBookRating, EBookRatingDto>();
    }
}