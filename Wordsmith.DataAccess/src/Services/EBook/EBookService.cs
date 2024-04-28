using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.EBook;
using Wordsmith.Models.RequestObjects.Statistics;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.EBookFileHelper;

namespace Wordsmith.DataAccess.Services.EBook;

public class EBookService : WriteService<EBookDto, Db.Entities.EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>, IEBookService
{
    public EBookService(DatabaseContext context, IMapper mapper) : base(context,
        mapper)
    {
    }

    protected override async Task BeforeInsert(Db.Entities.EBook entity, EBookInsertRequest insert, int userId)
    {
        await ValidateForeignKeys(insert);
        await HandleImage(entity, insert);
        
        entity.PublishedDate = DateTime.UtcNow;
        entity.UpdatedDate = entity.PublishedDate;

        var fileName = await EBookFileHelper.SaveFile(insert.file);
        entity.Path = fileName;
    }

    protected override async Task AfterInsert(Db.Entities.EBook entity, EBookInsertRequest insert, int userId)
    {
        await HandleLibraryAdd(entity, insert);
        await HandleChapters(entity, insert);
        await HandleGenres(entity, insert);
        await Context.SaveChangesAsync();
        await Context.Entry(entity).Reference(e => e.MaturityRating).LoadAsync();
        await Context.Entry(entity).Reference(e => e.CoverArt).LoadAsync();
        await Context.Entry(entity).Reference(e => e.Author).LoadAsync();
    }

    protected override async Task AfterUpdate(Db.Entities.EBook entity, EBookUpdateRequest update, int userId)
    {
        await Context.Entry(entity).Reference(e => e.MaturityRating).LoadAsync();
        await Context.Entry(entity).Reference(e => e.CoverArt).LoadAsync();
        await Context.Entry(entity).Reference(e => e.Author).LoadAsync();
    }

    protected override IQueryable<Db.Entities.EBook> AddFilter(IQueryable<Db.Entities.EBook> query,
        EBookSearchObject search, int userId)
    {
        query = query.Where(e => !e.IsHidden);
        
        if (search.Title != null)
        {
            query = query.Where(e => e.Title.Contains(search.Title, StringComparison.InvariantCultureIgnoreCase));
        }

        if (search.Genres != null)
        {
            query = query.Where(e => e.EBookGenres.Any(g => search.Genres.Contains(g.GenreId)));
        }

        if (search.MaturityRatingId != null)
        {
            query = query.Where(e => e.MaturityRatingId == search.MaturityRatingId.Value);
        }

        if (search.AuthorId.HasValue)
        {
            query = query.Where(e => e.AuthorId == search.AuthorId.Value);
        }

        return query;
    }

    protected override IQueryable<Db.Entities.EBook> AddInclude(IQueryable<Db.Entities.EBook> query, int userId)
    {
        query = query.Include(e => e.MaturityRating).Include(e => e.CoverArt).Include(e => e.Author);
    
        return query;
    }
    
    public async Task<EntityResult<EBookParseDto>> Parse(IFormFile file)
    {
        var data = await EBookFileHelper.ParseEpub(file);

        return new EntityResult<EBookParseDto>()
        {
            Message = "Parsed eBook",
            Result = data
        };
    }

    public async Task<EBookFile> Download(int id)
    {
        var entity = await Context.EBooks.FindAsync(id);

        if (entity == null)
        {
            throw new AppException("The ebook does not exist!");
        }

        var ebookFile = await EBookFileHelper.GetFile(entity.Path);

        return ebookFile;
    }

    public async Task<EntityResult<EBookDto>> Hide(int id)
    {
        await ValidateHiding(id);

        var ebook = await Context.EBooks
            .Include(e => e.MaturityRating)
            .Include(e => e.CoverArt)
            .Include(e => e.Author)
            .SingleAsync(e => e.Id == id);
        ebook.IsHidden = true;
        ebook.HiddenDate = DateTime.UtcNow;

        await Context.SaveChangesAsync();
        
        return new EntityResult<EBookDto>()
        {
            Message = "Ebook is hidden!",
            Result = Mapper.Map<EBookDto>(ebook)
        };
    }

    public async Task<EntityResult<EBookDto>> Unhide(int id)
    {
        await ValidateHiding(id);

        var ebook = await Context.EBooks
            .Include(e => e.MaturityRating)
            .Include(e => e.CoverArt)
            .Include(e => e.Author)
            .SingleAsync(e => e.Id == id);
        ebook.IsHidden = false;
        ebook.HiddenDate = null;

        await Context.SaveChangesAsync();
        
        return new EntityResult<EBookDto>()
        {
            Message = "Ebook is unhidden!",
            Result = Mapper.Map<EBookDto>(ebook)
        };
    }
    
    public async Task<QueryResult<EBookPublishStatisticsDto>> GetPublishStatistics(StatisticsRequest request)
    {
        request.StartDate = request.StartDate.ToUniversalTime();
        request.EndDate = request.EndDate.ToUniversalTime();
        
        var isHourlyGranularity = StatisticsHelper.IsHourlyGranularity(request.StartDate, request.EndDate);
        
        var publishesDict = await Context.EBooks
            .Where(e => e.PublishedDate.Date >= request.StartDate.Date &&
                        e.PublishedDate.Date <= request.EndDate.Date)
            .GroupBy(e => new DateTime(e.PublishedDate.Year, e.PublishedDate.Month, e.PublishedDate.Day,
                isHourlyGranularity ? e.PublishedDate.Hour : 0, 0, 0))
            .Select(g => new EBookPublishStatisticsDto()
            {
                Date = g.Key,
                PublishCount = g.Count()
            })
            .ToDictionaryAsync(e => e.Date);
        
        var result = new List<EBookPublishStatisticsDto>();
        var dateIncrement = StatisticsHelper.GetIncrementForGranularity(isHourlyGranularity);
        request.StartDate = StatisticsHelper.AdjustDateForGranularity(request.StartDate, isHourlyGranularity);
        request.EndDate = StatisticsHelper.AdjustDateForGranularity(request.EndDate, isHourlyGranularity);
        
        while (request.StartDate != request.EndDate)
        {
            if (publishesDict.TryGetValue(request.StartDate, out var statistic))
            {
                result.Add(statistic);
            }
            else
            {
                result.Add(new EBookPublishStatisticsDto()
                {
                    Date = request.StartDate,
                    PublishCount = 0
                });
            }
            
            request.StartDate += dateIncrement;
        }
        
        return new QueryResult<EBookPublishStatisticsDto>()
        {
            Result = result,
            Page = 1,
            TotalCount = result.Count,
            TotalPages = 1,
        };
    }
    
    public async Task<QueryResult<EBookTrafficStatisticsDto>> GetTrafficStatistics(StatisticsRequest request)
    {
        request.StartDate = request.StartDate.ToUniversalTime();
        request.EndDate = request.EndDate.ToUniversalTime();
        
        var result = await Context.UserLibraryHistories
            .Include(e => e.EBook)
            .Where(e => e.SyncDate.Date >= request.StartDate.Date &&
                        e.SyncDate.Date <= request.EndDate.Date)
            .GroupBy(e => e.EBook)
            .Select(g => new EBookTrafficStatisticsDto()
            {
                Id = g.Key.Id,
                Title = g.Key.Title,
                SyncCount = g.Count()
            })
            .OrderByDescending(g => g.SyncCount)
            .Take(request.PageSize)
            .ToListAsync();
        
        return new QueryResult<EBookTrafficStatisticsDto>()
        {
            Result = result,
            Page = 1,
            TotalCount = result.Count,
            TotalPages = 1,
        };
    }
    
    private async Task ValidateForeignKeys(EBookInsertRequest insert)
    {
        var authorExists = await Context.Users.AnyAsync(user => user.Id == insert.AuthorId);
        var maturityRatingExists =
            await Context.MaturityRatings.AnyAsync(rating => rating.Id == insert.MaturityRatingId);
        
        if (!authorExists) throw new AppException("The author for this eBook does not exist!");
        
        if (!maturityRatingExists) throw new AppException("The maturity rating for this eBook was not found!");
        
        foreach (var genreId in insert.GenreIds)
        {
            var genreExists = await Context.Genres.AnyAsync(genre => genre.Id == genreId);
            
            if (!genreExists) throw new AppException("The genre for this eBook was not found!");
        }
    }
    
    private async Task HandleImage(Db.Entities.EBook entity, EBookInsertRequest insert)
    {
        var savePath = Path.Combine("images", "ebooks", $"eBook-{Guid.NewGuid()}");
        var imageSaveInfo = ImageHelper.SaveFromBase64(insert.EncodedCoverArt, null, savePath);
        var newImage = new Image()
        {
            Format = imageSaveInfo.Format,
            Size = imageSaveInfo.Size,
            Path = imageSaveInfo.Path
        };

        await Context.Images.AddAsync(newImage);
        entity.CoverArt = newImage;
    }

    private async Task HandleChapters(Db.Entities.EBook entity, EBookInsertRequest insert)
    {
        var chapters = new List<Db.Entities.EBookChapter>();
        
        for (var i = 0; i < insert.Chapters.Count; i++)
        {
            var newChapter = new Db.Entities.EBookChapter()
            {
                ChapterName = insert.Chapters[i],
                ChapterNumber = i,
                EBookId = entity.Id
            };
            
            chapters.Add(newChapter);
        }

        await Context.EBookChapters.AddRangeAsync(chapters);
        entity.ChapterCount = chapters.Count;
    }

    private async Task HandleGenres(Db.Entities.EBook entity, EBookInsertRequest insert)
    {
        var eBookGenres = new List<EBookGenre>();

        foreach (var genreId in insert.GenreIds)
        {
            var genre = await Context.Genres.FindAsync(genreId);

            if (genre == null) throw new Exception($"Could not find the specified genre with id {genreId}");
            
            var newEBookGenre = new EBookGenre()
            {
                EBookId = entity.Id,
                GenreId = genreId, 
            };

            eBookGenres.Add(newEBookGenre);

            entity.Genres += $"{genre.Name};";
        }

        await Context.EBookGenres.AddRangeAsync(eBookGenres);
    }
    
    private async Task HandleLibraryAdd(Db.Entities.EBook entity, EBookInsertRequest insert)
    {
        var libraryInsert = new Db.Entities.UserLibrary()
        {
            EBookId = entity.Id,
            UserId = entity.AuthorId,
            SyncDate = DateTime.UtcNow,
        };
        
        await Context.UserLibraries.AddAsync(libraryInsert);
    }

    private async Task ValidateHiding(int id)
    {
        if (!await Context.EBooks.AnyAsync(e => e.Id == id))
        {
            throw new AppException("The requested ebook does not exist!");
        }
    }
}