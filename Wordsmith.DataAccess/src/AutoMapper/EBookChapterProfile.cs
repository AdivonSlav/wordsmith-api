using AutoMapper;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;

namespace Wordsmith.DataAccess.AutoMapper;

public class EBookChapterProfile : Profile
{
    public EBookChapterProfile()
    {
        CreateMap<EBookChapter, EBookChapterDto>();
    }
}