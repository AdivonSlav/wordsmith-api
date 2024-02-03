using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class MaturityRatingProfile : Profile
{
    public MaturityRatingProfile()
    {
        CreateMap<MaturityRating, MaturityRatingDto>();
    }
}