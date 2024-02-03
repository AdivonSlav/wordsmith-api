using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class UserLibraryProfile : Profile
{
    public UserLibraryProfile()
    {
        CreateMap<UserLibrary, UserLibraryDto>();
        CreateMap<UserLibraryInsertRequest, UserLibrary>();
        CreateMap<UserLibraryUpdateRequest, UserLibrary>();
    }
}