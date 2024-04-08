using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.Services;

public interface IWriteService<T, TDb, TSearch, TInsert, TUpdate> : IReadService<T, TSearch>
    where T : class
    where TDb : class
    where TSearch : class
    where TInsert : class
    where TUpdate : class
{
    Task<EntityResult<T>> Insert(TInsert insert, int userId);
    Task<EntityResult<T>> Update(int id, TUpdate update, int userId);
    Task<EntityResult<T>> Delete(int userId, params int[] ids);
}