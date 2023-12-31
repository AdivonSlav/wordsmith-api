using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.SearchObjects;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable IDE0058 // Nullable reference

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

    public virtual async Task<ActionResult<T>> Insert(TInsert insert)
    {
        var set = Context.Set<TDb>();
        var entity = Mapper.Map<TDb>(insert);

        await set.AddAsync(entity);
        
        await BeforeInsert(entity, insert);
        await Context.SaveChangesAsync();
        await AfterInsert(entity, insert);

        return new CreatedAtActionResult(null, null, null, Mapper.Map<T>(entity));
    }

    public virtual async Task<ActionResult<T>> Update(int id, TUpdate update)
    {
        var set = Context.Set<TDb>();
        var entity = await set.FindAsync(id);

        if (entity == null)
        {
            throw new AppException("The entity passed for updating was not found!");
        }
        
        await BeforeUpdate(entity, update);
        Mapper.Map(update, entity);

        await Context.SaveChangesAsync();
        await AfterUpdate(entity, update);
        
        return new OkObjectResult(Mapper.Map<T>(entity));
    }

    // A task that needs to be done before a DB add operation is finalized
    protected virtual async Task BeforeInsert(TDb entity, TInsert insert) { }

    // A task that needs to be done after a DB add operation is finalized
    protected virtual async Task AfterInsert(TDb entity, TInsert insert) { }

    protected virtual async Task BeforeUpdate(TDb entity, TUpdate update) { }
    
    protected virtual async Task AfterUpdate(TDb entity, TUpdate update) { }
}