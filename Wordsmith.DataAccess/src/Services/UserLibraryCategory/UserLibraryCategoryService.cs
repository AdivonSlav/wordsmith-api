using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class UserLibraryCategoryService : ReadService<UserLibraryCategoryDto, UserLibraryCategory, UserLibraryCategorySearchObject>, IUserLibraryCategoryService
{
    public UserLibraryCategoryService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}