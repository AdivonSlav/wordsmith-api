using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.UserLibraryCategory;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserLibraryCategory;

public class UserLibraryCategoryService : WriteService<UserLibraryCategoryDto, Db.Entities.UserLibraryCategory, UserLibraryCategorySearchObject, UserLibraryCategoryInsertRequest, UserLibraryCategoryUpdateRequest>, IUserLibraryCategoryService
{
    public UserLibraryCategoryService(DatabaseContext context, IMapper mapper) : base(context, mapper)
    {
    }
    
    public async Task<EntityResult<UserLibraryCategoryDto>> AddToCategory(UserLibraryCategoryInsertRequest insertRequest)
    {
        await ValidateCategoryAdd(insertRequest);

        var result = new EntityResult<UserLibraryCategoryDto>();

        if (insertRequest.UserLibraryCategoryId.HasValue)
        {
            var category = await LibraryEntryCategoryExists(insertRequest.UserLibraryCategoryId.Value);
            
            await AssignCategoryToLibraryEntries(insertRequest.UserLibraryIds, category, insertRequest.UserId);

            result.Message = "Added the entries to the category";
            result.Result = Mapper.Map<UserLibraryCategoryDto>(category);
        }
        else if (!string.IsNullOrEmpty(insertRequest.NewCategoryName))
        {
            if (!await Context.Users.AnyAsync(u => u.Id == insertRequest.UserId))
            {
                throw new AppException("The user of the library entry does not exist!");
            }
            
            var newCategory = new Db.Entities.UserLibraryCategory
            {
                Name = insertRequest.NewCategoryName,
                UserId = insertRequest.UserId
            };

            await AssignCategoryToLibraryEntries(insertRequest.UserLibraryIds, newCategory, insertRequest.UserId);

            result.Message = "Created a new category and added the requested entries";
            result.Result = Mapper.Map<UserLibraryCategoryDto>(newCategory);
        }

        return result;
    }

    public async Task<EntityResult<UserLibraryCategoryDto>> RemoveFromCategory(UserLibraryCategoryRemoveRequest removeRequest)
    {
        await ValidateRemoveFromCategory(removeRequest);
        await RemoveCategoryFromEntries(removeRequest.UserLibraryIds, removeRequest.UserId);

        return new EntityResult<UserLibraryCategoryDto>()
        {
            Message = "Successfully removed category from entries!"
        };
    }

    private async Task ValidateCategoryAdd(UserLibraryCategoryInsertRequest insertRequest)
    {
        if (insertRequest.UserLibraryCategoryId == null && string.IsNullOrEmpty(insertRequest.NewCategoryName))
        {
            throw new AppException(
                "You must either add to an existing category or provide a name for a new one to be created!");
        }

        if (!string.IsNullOrEmpty(insertRequest.NewCategoryName))
        {
            if (await Context.UserLibraryCategories.AnyAsync(c => c.Name == insertRequest.NewCategoryName))
            {
                throw new AppException("Category with the same name already exists!");
            }
        }

        var libraryEntryCount = await Context.UserLibraries.CountAsync(e => insertRequest.UserLibraryIds.Contains(e.Id));

        if (libraryEntryCount != insertRequest.UserLibraryIds.Count)
        {
            throw new AppException("Some of the library entries don't exist!");
        }
    }

    private async Task ValidateRemoveFromCategory(UserLibraryCategoryRemoveRequest removeRequest)
    {
        var libraryEntryCount = await Context.UserLibraries.CountAsync(e => removeRequest.UserLibraryIds.Contains(e.Id));
        
        if (libraryEntryCount != removeRequest.UserLibraryIds.Count)
        {
            throw new AppException("Some of the library entries don't exist!");
        }
    }

    private async Task AssignCategoryToLibraryEntries(IEnumerable<int> libraryEntries,
        Db.Entities.UserLibraryCategory category, int userId)
    {
        var entries = await Context.UserLibraries.Where(e => libraryEntries.Contains(e.Id)).ToListAsync();
        
        foreach (var entry in entries)
        {
            if (userId != entry.UserId)
            {
                throw new AppException("Cannot change categories for library entries that you do not own!");
            }
            
            entry.UserLibraryCategory = category;
        }

        await Context.SaveChangesAsync();
    }

    private async Task RemoveCategoryFromEntries(IEnumerable<int> libraryEntries, int userId)
    {
        var entries = await Context.UserLibraries.Where(e => libraryEntries.Contains(e.Id)).ToListAsync();
        
        foreach (var entry in entries)
        {
            if (userId != entry.UserId)
            {
                throw new AppException("Cannot change categories for library entries that you do not own!");
            }

            entry.UserLibraryCategoryId = null;
        }

        await Context.SaveChangesAsync();
    }
    
    private async Task<Db.Entities.UserLibraryCategory> LibraryEntryCategoryExists(int categoryId)
    {
        var category = await Context.UserLibraryCategories.FindAsync(categoryId);

        return category ?? throw new AppException("This category does not exist!");
    }
}