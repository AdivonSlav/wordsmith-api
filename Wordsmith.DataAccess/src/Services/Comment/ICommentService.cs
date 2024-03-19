using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Comment;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Comment;

public interface ICommentService : IWriteService<CommentDto, Db.Entities.Comment, CommentSearchObject, CommentInsertRequest, CommentUpdateRequest>
{
    
}