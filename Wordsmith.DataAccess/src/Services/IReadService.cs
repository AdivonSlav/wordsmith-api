using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IReadService<T, TSearch>
    where T : class
    where TSearch : class
{
    Task<QueryResult<T>> Get(TSearch search, int userId);
    Task<QueryResult<T>> GetById(int id, int userId);
}