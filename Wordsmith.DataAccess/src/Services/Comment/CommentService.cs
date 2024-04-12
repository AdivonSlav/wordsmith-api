using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.Comment;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils.ProfanityDetector;

namespace Wordsmith.DataAccess.Services.Comment;

public class CommentService : WriteService<CommentDto, Db.Entities.Comment, CommentSearchObject, CommentInsertRequest, CommentUpdateRequest>, ICommentService
{
    private readonly IProfanityDetector _profanityDetector;

    public CommentService(DatabaseContext context, IMapper mapper, IProfanityDetector profanityDetector) : base(context,
        mapper)
    {
        _profanityDetector = profanityDetector;
    }

    protected override async Task BeforeInsert(Db.Entities.Comment entity, CommentInsertRequest insert, int userId)
    {
        await ValidateInsertion(entity, insert);
        entity.DateAdded = DateTime.UtcNow;
        entity.IsShown = true;
        await Context.Entry(entity).Reference(e => e.EBookChapter).LoadAsync();
        await Context.Entry(entity).Reference(e => e.User).LoadAsync();
        await Context.Entry(entity.User).Reference(e => e.ProfileImage).LoadAsync();
    }
    
    protected override Task BeforeDeletion(int userId, Db.Entities.Comment entity)
    {
        ValidateDeletion(userId, entity);

        return Task.CompletedTask;
    }

    protected override IQueryable<Db.Entities.Comment> AddFilter(IQueryable<Db.Entities.Comment> query,
        CommentSearchObject search, int userId)
    {
        if (search.EBookId.HasValue)
        {
            query = query.Where(e => e.EBookId == search.EBookId.Value);
        }

        if (search.UserId.HasValue)
        {
            query = query.Where(e => e.UserId == search.UserId);
        }

        if (search.IsShown.HasValue)
        {
            query = query.Where(e => e.IsShown == search.IsShown);
        }

        if (search.EBookChapterId.HasValue)
        {
            query = query.Where(e => e.EBookChapterId == search.EBookChapterId.Value);
        }

        return query;
    }

    protected override IQueryable<Db.Entities.Comment> AddInclude(IQueryable<Db.Entities.Comment> query, int userId)
    {
        query = query
            .Include(e => e.User)
            .ThenInclude(e => e.ProfileImage)
            .Include(e => e.EBookChapter);

        return query;
    }

    protected override async Task<CommentDto> EditGetResponse(CommentDto dto, int userId)
    {
        dto.HasLiked = await Context.CommentLikes.AnyAsync(e => e.UserId == userId && e.CommentId == dto.Id);

        return dto;
    }

    public async Task<EntityResult<CommentDto>> Like(int id, int userId)
    {
        await ValidateLike(id, userId);

        var comment = await Context.Comments
            .Include(e => e.User)
            .ThenInclude(e => e.ProfileImage)
            .Include(e => e.EBookChapter)
            .FirstAsync(e => e.Id == id);
        var newCommentLike = new CommentLike()
        {
            UserId = userId,
            CommentId = id,
            LikeDate = DateTime.UtcNow
        };
        
        await Context.CommentLikes.AddAsync(newCommentLike);
        comment.LikeCount++;
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<CommentDto>(comment);
        dto.HasLiked = true;

        return new EntityResult<CommentDto>()
        {
            Message = "Successfully liked comment",
            Result = dto
        };
    }

    public async Task<EntityResult<CommentDto>> RemoveLike(int id, int userId)
    {
        await ValidateRemoveLike(id, userId);

        var comment = await Context.Comments
            .Include(e => e.User)
            .ThenInclude(e => e.ProfileImage)
            .Include(e => e.EBookChapter)
            .FirstAsync(e => e.Id == id);
        var like = await Context.CommentLikes.FirstAsync(e => e.CommentId == id && e.UserId == userId);
        
        Context.CommentLikes.Remove(like);
        comment.LikeCount--;
        await Context.SaveChangesAsync();

        var dto = Mapper.Map<CommentDto>(comment);
        dto.HasLiked = false;
        
        return new EntityResult<CommentDto>()
        {
            Message = "Successfully deleted like for comment",
            Result = dto
        };
    }

    private async Task ValidateInsertion(Db.Entities.Comment comment, CommentInsertRequest insert)
    {
        if (!await Context.EBooks.AnyAsync(e => e.Id == insert.EBookId))
        {
            throw new AppException("The ebook you are trying to comment on does not exist!");
        }

        if (!await Context.Users.AnyAsync(e => e.Id == insert.UserId))
        {
            throw new AppException("The user trying to comment does not exist!");
        }

        if (!await Context.UserLibraries.AnyAsync(e => e.EBookId == insert.EBookId && e.UserId == insert.UserId))
        {
            throw new AppException("You cannot comment on an ebook you do not have in your library!");
        }

        if (insert.EBookChapterId.HasValue &&
            !await Context.EBookChapters.AnyAsync(e => e.Id == insert.EBookChapterId.Value))
        {
            throw new AppException("The ebook chapter you are trying to comment on does not exist!");
        }

        if (_profanityDetector.ContainsProfanity(comment.Content))
        {
            comment.Content = _profanityDetector.Censor(comment.Content);
        }
    }

    private void ValidateDeletion(int userId, Db.Entities.Comment comment)
    {
        if (comment.UserId != userId)
        {
            throw new AppException("You cannot delete a comment you did not post!");
        }
    }

    private async Task ValidateLike(int id, int userId)
    {
        if (!await Context.Comments.AnyAsync(e => e.Id == id))
        {
            throw new AppException("The selected comment does not exist!");
        }

        if (!await Context.Users.AnyAsync(e => e.Id == userId))
        {
            throw new AppException("The user trying to like does not exist!");
        }

        if (await Context.CommentLikes.AnyAsync(e => e.UserId == userId && e.CommentId == id))
        {
            throw new AppException("You have already liked this comment!");
        }
    }

    private async Task ValidateRemoveLike(int id, int userId)
    {
        if (!await Context.Users.AnyAsync(e => e.Id == userId))
        {
            throw new AppException("The user trying to like does not exist!");
        }
        
        var comment = await Context.Comments.FindAsync(id);
        
        if (comment == null)
        {
            throw new AppException("The selected comment does not exist!");
        }

        if (comment.UserId != userId)
        {
            throw new AppException("You cannot remove a like for a comment you have not made!");
        }
        
        if (!await Context.CommentLikes.AnyAsync(e => e.CommentId == id && e.UserId == userId))
        {
            throw new AppException("The user has not liked this comment!");
        }
    }
}