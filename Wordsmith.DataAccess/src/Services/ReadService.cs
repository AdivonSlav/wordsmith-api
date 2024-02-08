using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Extensions;
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

    protected ReadService(DatabaseContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }

    public virtual async Task<QueryResult<T>> Get(TSearch search)
    {
        var query = Context.Set<TDb>().AsQueryable();
        var result = new QueryResult<T>();

        query = AddInclude(query, search);
        query = AddFilter(query, search);

        result.TotalCount = await query.CountAsync();
        query = query.Skip((search.Page - 1) * search.PageSize).Take(search.PageSize);
        result.Page = search.Page;
        result.TotalPages = (int)Math.Ceiling((double)result.TotalCount / (double)search.PageSize);
        
        if (search?.OrderBy != null)
        {
            var orderByParts = search.OrderBy.Split(":");

            if (orderByParts.Length < 2)
            {
                throw new AppException("You must pass both a property and direction!");
            }
            
            var orderByProperty = orderByParts[0];
            var orderByDirection = orderByParts[1];

            if (orderByDirection == "asc")
            {
                query = query.OrderByProperty(orderByProperty);
            } 
            else if (orderByDirection == "desc")
            {
                query = query.OrderByProperty(orderByProperty, false);
            }
            else
            {
                throw new AppException("OrderBy must be asc or desc");
            }
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
            throw;
        }

        return result;
    }

    public virtual async Task<QueryResult<T>> GetById(int id)
    {
        var entity = await Context.Set<TDb>().FindAsync(id);
        var result = new QueryResult<T>()
        {
            Result = new List<T>() { Mapper.Map<T>(entity) }
        };
        
        return result;
    }

    protected virtual IQueryable<TDb> AddInclude(IQueryable<TDb> query, TSearch search)
    {
        return query;
    }

    protected virtual IQueryable<TDb> AddFilter(IQueryable<TDb> query, TSearch search)
    {
        return query;
    }
}