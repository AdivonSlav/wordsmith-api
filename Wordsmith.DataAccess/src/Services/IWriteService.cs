namespace Wordsmith.DataAccess.Services;

public interface IWriteService<T, TDb, TSearch, TInsert, TUpdate> : IReadService<T, TSearch>
    where T : class
    where TDb : class
    where TSearch : class
    where TInsert : class
    where TUpdate : class
{
    Task<T> Insert(TInsert insert);
    Task<T> Update(int id, TUpdate update);
}