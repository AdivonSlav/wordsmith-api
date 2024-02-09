using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class UserLibraryCategoryProfile : Profile
{
    public UserLibraryCategoryProfile()
    {
        CreateMap<UserLibraryCategory, UserLibraryCategoryDto>();
    }
}