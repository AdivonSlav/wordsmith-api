using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.EBookFileHelper;

namespace Wordsmith.DataAccess.Services;

public class EBookService : WriteService<EBookDto, EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>, IEBookService
{
    public EBookService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override async Task BeforeInsert(EBook entity, EBookInsertRequest insert)
    {
        await ValidateForeignKeys(insert);
        await HandleImage(entity, insert);
        
        entity.PublishedDate = DateTime.UtcNow;
        entity.UpdatedDate = entity.PublishedDate;
        entity.RatingAverage = 0.0;

        var fileName = await EBookFileHelper.SaveFile(insert.file);
        entity.Path = fileName;
    }

    protected override async Task AfterInsert(EBook entity, EBookInsertRequest insert)
    {
        await HandleChapters(entity, insert);
        await HandleGenres(entity, insert);
        await Context.SaveChangesAsync();
        await Context.Entry(entity).Reference(e => e.MaturityRating).LoadAsync();
        await Context.Entry(entity).Reference(e => e.CoverArt).LoadAsync();
        await Context.Entry(entity).Reference(e => e.Author).LoadAsync();
    }

    protected override IQueryable<EBook> AddFilter(IQueryable<EBook> query, EBookSearchObject search = null)
    {
        if (search?.Title != null)
        {
            query = query.Where(e => e.Title.Contains(search.Title, StringComparison.InvariantCultureIgnoreCase));
        }

        if (search?.Genres != null)
        {
            query = query.Where(e => e.EBookGenres.Any(g => search.Genres.Contains(g.GenreId)));
        }

        if (search?.MaturityRatingId != null)
        {
            query = query.Where(e => e.MaturityRatingId == search.MaturityRatingId.Value);
        }

        return query;
    }

    protected override IQueryable<EBook> AddInclude(IQueryable<EBook> query, EBookSearchObject search = null)
    {
        query = query.Include(e => e.MaturityRating).Include(e => e.CoverArt).Include(e => e.Author);

        return query;
    }

    public async Task<ActionResult<EBookParseDto>> Parse(IFormFile file)
    {
        var data = await EBookFileHelper.ParseEpub(file);

        return new OkObjectResult(data);
    }

    public async Task<IActionResult> Download(int id)
    {
        var entity = await Context.EBooks.FindAsync(id);

        if (entity == null)
        {
            throw new AppException("The ebook does not exist!");
        }

        var ebookFile = await EBookFileHelper.GetFile(entity.Path);

        return new FileContentResult(ebookFile.Bytes, MediaTypeHeaderValue.Parse("application/epub+zip"))
        {
            FileDownloadName = ebookFile.Filename
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

    private async Task HandleImage(EBook entity, EBookInsertRequest insert)
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

    private async Task HandleChapters(EBook entity, EBookInsertRequest insert)
    {
        var chapters = new List<EBookChapter>();
        
        for (var i = 0; i < insert.Chapters.Count; i++)
        {
            var newChapter = new EBookChapter()
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

    private async Task HandleGenres(EBook entity, EBookInsertRequest insert)
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
}