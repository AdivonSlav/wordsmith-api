using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IReadService<T, TSearch>
    where T : class
    where TSearch : class
{
    Task<PaginatedResult<T>> Get(TSearch? search = null);
    Task<T> GetById(int id);
}