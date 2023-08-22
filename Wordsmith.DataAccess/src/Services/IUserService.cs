using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IUserService : IWriteService<UserDto, User, SearchObject, UserInsertRequest, UserUpdateRequest>
{
    
}