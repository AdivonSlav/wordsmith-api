using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
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

    protected override async Task BeforeInsert(Db.Entities.Comment entity, CommentInsertRequest insert)
    {
        await ValidateInsertion(entity, insert);
        entity.DateAdded = DateTime.UtcNow;
        entity.IsShown = true;
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
}