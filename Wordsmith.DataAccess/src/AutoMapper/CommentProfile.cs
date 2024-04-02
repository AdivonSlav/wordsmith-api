using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Comment;

namespace Wordsmith.DataAccess.AutoMapper;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentInsertRequest, Comment>();
        CreateMap<CommentUpdateRequest, Comment>();
        CreateMap<Comment, CommentDto>();
    }
}