using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookChapter;

public interface IEBookChapterService : IReadService<EBookChapterDto, EBookChapterSearchObject>
{
}