#nullable enable
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Extensions;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Services;

public class ReadService<T, TDb, TSearch> : IReadService<T, TSearch>
    where TDb : class, IEntity
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

    public virtual async Task<QueryResult<T>> Get(TSearch search, int userId)
    {
        var query = Context.Set<TDb>().AsQueryable();
        var result = new QueryResult<T>();

        query = AddInclude(query, userId);
        query = AddFilter(query, search, userId);

        if (search.OrderBy != null)
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
        
        result.TotalCount = await query.CountAsync();
        query = query.Skip((search!.Page - 1) * search.PageSize).Take(search.PageSize);
        result.Page = search.Page;
        result.TotalPages = (int)Math.Ceiling((double)result.TotalCount / search.PageSize);

        try
        {
            var list = await query.ToListAsync();
            var tmp = Mapper.Map<List<T>>(list);

            for (var i = 0; i < tmp.Count; i++)
            {
                tmp[i] = await EditGetResponse(tmp[i], userId);
            }
            
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

    public virtual async Task<QueryResult<T>> GetById(int id, int userId)
    {
        var query = Context.Set<TDb>().AsQueryable();
        var result = new QueryResult<T>();

        query = AddInclude(query, userId);
        
        try
        {
            var entity = await query.FirstOrDefaultAsync(e => e.Id == id);

            if (entity != null)
            {
                var tmp = Mapper.Map<T>(entity);
                tmp = await EditGetResponse(tmp, userId);
                result.Result.Add(tmp);
            }
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

    protected virtual IQueryable<TDb> AddInclude(IQueryable<TDb> query, int userId)
    {
        return query;
    }

    protected virtual IQueryable<TDb> AddFilter(IQueryable<TDb> query, TSearch search, int userId)
    {
        return query;
    }

    protected virtual Task<T> EditGetResponse(T dto, int userId)
    {
        return Task.FromResult(dto);
    }
}