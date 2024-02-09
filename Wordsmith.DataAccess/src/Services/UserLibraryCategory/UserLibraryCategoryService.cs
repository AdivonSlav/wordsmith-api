using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserLibraryCategory;

public class UserLibraryCategoryService : ReadService<UserLibraryCategoryDto, Db.Entities.UserLibraryCategory, UserLibraryCategorySearchObject>, IUserLibraryCategoryService
{
    public UserLibraryCategoryService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}