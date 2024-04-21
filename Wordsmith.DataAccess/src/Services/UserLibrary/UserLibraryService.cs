using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Enums;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.UserLibrary;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserLibrary;

public class UserLibraryService : WriteService<UserLibraryDto, Db.Entities.UserLibrary, UserLibrarySearchObject, UserLibraryInsertRequest, UserLibraryUpdateRequest>, IUserLibraryService
{
    public UserLibraryService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override async Task BeforeInsert(Db.Entities.UserLibrary entity, UserLibraryInsertRequest insert,
        int userId)
    {
        await ValidateInsertion(insert);
        
        entity.SyncDate = DateTime.UtcNow;
    }

    protected override async Task AfterInsert(Db.Entities.UserLibrary entity, UserLibraryInsertRequest insert,
        int userId)
    {
        await Context.Entry(entity).Reference(e => e.EBook).LoadAsync();
        await Context.Entry(entity.EBook).Reference(e => e.Author).LoadAsync();
        await Context.Entry(entity.EBook).Reference(e => e.CoverArt).LoadAsync();
        await Context.Entry(entity.EBook).Reference(e => e.MaturityRating).LoadAsync();
    }

    private async Task ValidateInsertion(UserLibraryInsertRequest insert)
    {
        var ebook = await Context.EBooks.Include(e => e.Author).FirstOrDefaultAsync(e => e.Id == insert.EBookId);
        
        if (ebook == null)
        {
            throw new AppException("The ebook does not exist");
        }

        if (ebook.Author.Status != UserStatus.Active)
        {
            throw new AppException("You cannot add this ebook to your library as the author has been banned!");
        }

        var alreadyAdded = await Context.UserLibraries.AnyAsync(e => e.UserId == insert.UserId && e.EBookId == insert.EBookId);

        if (alreadyAdded)
        {
            throw new AppException("Book is already added to your library");
        }

        if (ebook.Price.HasValue)
        {
            var order = await Context.Orders.FirstOrDefaultAsync(e => e.ReferenceId == insert.OrderReferenceId);

            if (order != null)
            {
                if (order.EBookId != ebook.Id || order.PayerId != insert.UserId ||
                    order.Status != OrderStatus.Completed)
                {
                    throw new AppException("You have not purchased the ebook!");
                }
            }
        }
    }
    
    protected override async Task BeforeDeletion(int userId, Db.Entities.UserLibrary entity)
    {
        await ValidateDeletion(userId, entity);
    }
    
    protected override IQueryable<Db.Entities.UserLibrary> AddInclude(IQueryable<Db.Entities.UserLibrary> query,
        int userId)
    {
        query = query
            .Include(e => e.EBook)
            .ThenInclude(b => b.Author)
            .Include(e => e.EBook)
            .ThenInclude(b => b.CoverArt)
            .Include(e => e.EBook)
            .ThenInclude(b => b.MaturityRating);
    
        return query;
    }

    protected override IQueryable<Db.Entities.UserLibrary> AddFilter(IQueryable<Db.Entities.UserLibrary> query,
        UserLibrarySearchObject search, int userId)
    {
        query = query.Where(e => e.UserId == search.UserId);
        query = query.Where(e => !e.EBook.IsHidden);

        if (search.IsRead.HasValue)
        {
            query = query.Where(e => e.IsRead == search.IsRead.Value);
        }
        
        if (search.MaturityRatingId.HasValue)
        {
            query = query.Where(e => e.EBook.MaturityRatingId == search.MaturityRatingId.Value);
        }

        if (search.EBookId.HasValue)
        {
            query = query.Where(e => e.EBookId == search.EBookId.Value);
        }

        if (search.UserLibraryCategoryId.HasValue)
        {
            query = query.Where(e => e.UserLibraryCategoryId == search.UserLibraryCategoryId.Value);
        }

        return query;
    }
    
    private Task ValidateDeletion(int userId, Db.Entities.UserLibrary entity)
    {
        if (entity.EBook.AuthorId == userId)
        {
            throw new AppException("You cannot remove a book you published from your library!");
        }
        
        return Task.CompletedTask;
    }
}