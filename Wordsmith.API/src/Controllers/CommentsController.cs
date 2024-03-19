using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.Comment;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Comment;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("comments")]
public class CommentsController : WriteController<CommentDto, Comment, CommentSearchObject, CommentInsertRequest, CommentUpdateRequest>
{
    public CommentsController(ICommentService commentService) : base(commentService) { }

    [SwaggerOperation("Insert a new comment for an ebook or chapter")]
    [Authorize("All")]
    public override Task<ActionResult<EntityResult<CommentDto>>> Insert(CommentInsertRequest insert)
    {
        insert.UserId = GetAuthUserId();
        return base.Insert(insert);
    }

    [SwaggerOperation("Update a comment")]
    [Authorize("Admin")]
    public override Task<ActionResult<EntityResult<CommentDto>>> Update(int id, CommentUpdateRequest update)
    {
        return base.Update(id, update);
    }
}