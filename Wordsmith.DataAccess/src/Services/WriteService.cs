using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.SearchObjects;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable IDE0058 // Nullable reference

namespace Wordsmith.DataAccess.Services;

public class WriteService<T, TDb, TSearch, TInsert, TUpdate> : ReadService<T, TDb, TSearch>, IWriteService<T, TDb, TSearch, TInsert, TUpdate>
    where T : class
    where TDb : class, IEntity
    where TSearch : SearchObject
    where TInsert : class
    where TUpdate : class
{
    public WriteService(DatabaseContext context, IMapper mapper)
        : base(context, mapper) { }

    public virtual async Task<EntityResult<T>> Insert(TInsert insert, int userId)
    {
        var set = Context.Set<TDb>();
        var entity = Mapper.Map<TDb>(insert);

        await using var transaction = await Context.Database.BeginTransactionAsync();

        try
        {
            await set.AddAsync(entity);

            await BeforeInsert(entity, insert, userId);
            await Context.SaveChangesAsync();
            await AfterInsert(entity, insert, userId);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new EntityResult<T>()
        {
            Message = "Successfully created",
            Result = Mapper.Map<T>(entity)
        };
    }

    public virtual async Task<EntityResult<T>> Update(int id, TUpdate update, int userId)
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
            await BeforeUpdate(entity, update, userId);
            Mapper.Map(update, entity);

            await Context.SaveChangesAsync();
            await AfterUpdate(entity, update, userId);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        
        return new EntityResult<T>()
        {
            Message = "Successfully updated",
            Result = Mapper.Map<T>(entity)
        };
    }

    public virtual async Task<EntityResult<T>> Delete(int userId, params int[] ids)
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
            await BeforeDeletion(userId, entity);
            set.Remove(entity);
            await Context.SaveChangesAsync();
            await AfterDeletion(userId, entity);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new EntityResult<T>()
        {
            Message = "Successfully deleted"
        };
    }

    // A task that needs to be done before a DB add operation is finalized
    protected virtual async Task BeforeInsert(TDb entity, TInsert insert, int userId) { }

    // A task that needs to be done after a DB add operation is finalized
    protected virtual async Task AfterInsert(TDb entity, TInsert insert, int userId) { }

    // A task that needs to be done before a DB update operation is finalized
    protected virtual async Task BeforeUpdate(TDb entity, TUpdate update, int userId) { }
    
    // A task that needs to be done after a DB update operation is finalized
    protected virtual async Task AfterUpdate(TDb entity, TUpdate update, int userId) { }
    
    // A task that needs to be done before a DB delete operation is finalized
    protected virtual async Task BeforeDeletion(int userId, TDb entity) { }

    // A task that needs to be done after a DB delete operation is finalized
    protected virtual async Task AfterDeletion(int userId, TDb entity) { }
}