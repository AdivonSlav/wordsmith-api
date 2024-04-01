using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookChapter;

public class EBookChapterService : ReadService<EBookChapterDto, Db.Entities.EBookChapter, EBookChapterSearchObject>, IEBookChapterService
{
    public EBookChapterService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override IQueryable<Db.Entities.EBookChapter> AddFilter(IQueryable<Db.Entities.EBookChapter> query, EBookChapterSearchObject search, int userId)
    {
        if (search.EBookId.HasValue)
        {
            query = query.Where(e => e.EBookId == search.EBookId.Value);
        }

        if (search.ChapterName != null)
        {
            query = query.Where(e => e.ChapterName == search.ChapterName);
        }

        return query;
    }
}