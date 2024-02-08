using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Extensions;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.SearchObjects;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable IDE0058 // Nullable reference

namespace Wordsmith.DataAccess.Services;

public class WriteService<T, TDb, TSearch, TInsert, TUpdate> : ReadService<T, TDb, TSearch>, IWriteService<T, TDb, TSearch, TInsert, TUpdate>
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

        await using var transaction = await Context.Database.BeginTransactionAsync();

        try
        {
            await set.AddAsync(entity);

            await BeforeInsert(entity, insert);
            await Context.SaveChangesAsync();
            await AfterInsert(entity, insert);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        
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
        
        await using var transaction = await Context.Database.BeginTransactionAsync();

        try
        {
            await BeforeUpdate(entity, update);
            Mapper.Map(update, entity);

            await Context.SaveChangesAsync();
            await AfterUpdate(entity, update);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        
        return new OkObjectResult(Mapper.Map<T>(entity));
    }

    public virtual async Task<ActionResult<string>> Delete(params int[] ids)
    {
        var set = Context.Set<TDb>();
        var entity = await set.FindAsync(ids.Select(i => (object)i).ToArray());
        
        if (entity == null)
        {
            throw new AppException("The entity passed for deletion was not found!");
        }

        await using var transaction = await Context.Database.BeginTransactionAsync();

        try
        {
            set.Remove(entity);

            await BeforeDeletion(entity);
            await Context.SaveChangesAsync();
            await AfterDeletion(entity);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new OkObjectResult("Entity successfully deleted");
    }

    // A task that needs to be done before a DB add operation is finalized
    protected virtual async Task BeforeInsert(TDb entity, TInsert insert) { }

    // A task that needs to be done after a DB add operation is finalized
    protected virtual async Task AfterInsert(TDb entity, TInsert insert) { }

    // A task that needs to be done before a DB update operation is finalized
    protected virtual async Task BeforeUpdate(TDb entity, TUpdate update) { }
    
    // A task that needs to be done after a DB update operation is finalized
    protected virtual async Task AfterUpdate(TDb entity, TUpdate update) { }
    
    // A task that needs to be done before a DB delete operation is finalized
    protected virtual async Task BeforeDeletion(TDb entity) { }

    // A task that needs to be done after a DB delete operation is finalized
    protected virtual async Task AfterDeletion(TDb entity) { }
}