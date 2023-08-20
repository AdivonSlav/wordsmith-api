using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;

#pragma warning disable IDE0058

namespace Wordsmith.DataAccess.Services;

public class WriteService<T, TDb, TSearch, TInsert, TUpdate> : ReadService<T, TDb, TSearch>
    where T : class
    where TDb : class
    where TSearch : SearchObject
    where TInsert : class
    where TUpdate : class
{
    public WriteService(DatabaseContext context, IMapper mapper)
        : base(context, mapper) { }

    public virtual async Task<T> Insert(TInsert insert)
    {
        var set = Context.Set<TDb>();
        var entity = Mapper.Map<TDb>(insert);

        try
        {
            await set.AddAsync(entity);
            await BeforeInsert(entity, insert);
            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError("Failed to save changes to database", e);
            return null;
        }

        return Mapper.Map<T>(entity);
    }

    public virtual async Task<T> Update(int id, TUpdate update)
    {
        var set = Context.Set<TDb>();
        var entity = await set.FindAsync(id);

        Mapper.Map(update, entity);

        try
        {
            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError("Failed to save changes to database", e);
            return null;
        }

        return Mapper.Map<T>(entity);
    }

    // A task that needs to be done before a DB add operation is finalized
    protected virtual Task BeforeInsert(TDb entity, TInsert insert)
    {
        return Task.CompletedTask;
    }
}