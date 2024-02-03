using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDto>();
    }
}