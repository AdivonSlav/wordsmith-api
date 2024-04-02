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

    [SwaggerOperation("Delete a comment")]
    [Authorize("All")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<EntityResult<CommentDto>>> Delete(int id)
    {
        var userId = GetAuthUserId();
        return Ok(await (WriteService as ICommentService)!.Delete(userId, id));
    }

    [SwaggerOperation("Like a comment")]
    [Authorize("All")]
    [HttpPost("{id:int}/like")]
    public async Task<ActionResult<EntityResult<CommentDto>>> Like(int id)
    {
        var userId = GetAuthUserId();
        return Ok(await (WriteService as ICommentService)!.Like(id, userId));
    }

    [SwaggerOperation("Remove a like from a comment")]
    [Authorize("All")]
    [HttpDelete("{id:int}/like")]
    public async Task<ActionResult<EntityResult<CommentDto>>> RemoveLike(int id)
    {
        var userId = GetAuthUserId();
        return Ok(await (WriteService as ICommentService)!.RemoveLike(id, userId));
    }
}