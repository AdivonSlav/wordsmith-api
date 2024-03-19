using Microsoft.AspNetCore.Mvc;
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
}