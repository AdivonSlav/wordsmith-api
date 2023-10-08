using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Services;

public class ReadService<T, TDb, TSearch> : IReadService<T, TSearch>
    where TDb : class
    where T : class
    where TSearch : SearchObject
{
    protected readonly DatabaseContext Context;
    protected readonly IMapper Mapper;

    public ReadService(DatabaseContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public virtual async Task<QueryResult<T>> Get(TSearch? search = null)
    {
        var query = Context.Set<TDb>().AsQueryable();
        var result = new QueryResult<T>();

        query = AddInclude(query, search);
        query = AddFilter(query, search);

        if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
        {
            result.TotalCount = await query.CountAsync();
            query = query.Skip((search.Page.Value - 1) * search.PageSize.Value).Take(search.PageSize.Value);
            result.Page = search.Page;
            result.TotalPages = (int)Math.Ceiling((double)(result.TotalCount / search.PageSize));
        }

        try
        {
            var list = await query.ToListAsync();

            var tmp = Mapper.Map<List<T>>(list);
            result.Result = tmp;
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception e)
        {
            Logger.LogError("Failed to process query", e);
            return null;
        }

        return result;
    }

    public virtual async Task<T> GetById(int id)
    {
        var entity = await Context.Set<TDb>().FindAsync(id);

        return Mapper.Map<T>(entity);
    }

    protected virtual IQueryable<TDb> AddInclude(IQueryable<TDb> query, TSearch? search = null)
    {
        return query;
    }

    protected virtual IQueryable<TDb> AddFilter(IQueryable<TDb> query, TSearch? search = null)
    {
        return query;
    }
}