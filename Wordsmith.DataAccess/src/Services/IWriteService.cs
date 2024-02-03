using Microsoft.AspNetCore.Mvc;

namespace Wordsmith.DataAccess.Services;

public interface IWriteService<T, TDb, TSearch, TInsert, TUpdate> : IReadService<T, TSearch>
    where T : class
    where TDb : class
    where TSearch : class
    where TInsert : class
    where TUpdate : class
{
    Task<ActionResult<T>> Insert(TInsert insert);
    Task<ActionResult<T>> Update(int id, TUpdate update);
    Task<ActionResult<string>> Delete(params int[] ids);
}