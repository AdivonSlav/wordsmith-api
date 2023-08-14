using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.DB;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services
{
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
            IQueryable<TDb> query = Context.Set<TDb>().AsQueryable();
            var result = new QueryResult<T>();

            query = AddInclude(query, search);
            query = AddFilter(query, search);

            if (search?.Page.HasValue == true && search?.PageSize.HasValue == true)
            {
                query = query.Take(search.PageSize.Value).Skip(search.Page.Value * search.PageSize.Value);
            }

            try
            {
                List<TDb> list = await query.ToListAsync();

                List<T> tmp = Mapper.Map<List<T>>(list);
                result.Result = tmp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e); // TODO: Replace with a logger call
                return null;
            }

            return result;
        }

        public virtual async Task<T> GetById(int id)
        {
            TDb entity = await Context.Set<TDb>().FindAsync(id);

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
}