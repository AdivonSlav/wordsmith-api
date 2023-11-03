using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Services;

public class EBookService : WriteService<EBookDto, EBook, EBookSearchObject, EBookInsertRequest, EBookUpdateRequest>, IEBookService
{
    public EBookService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override async Task BeforeInsert(EBook entity, EBookInsertRequest insert)
    {
        await ValidateForeignKeys(insert);
        ValidateIfSaved(insert);
        
        entity.PublishedDate = DateTime.UtcNow;
        entity.UpdatedDate = entity.PublishedDate;

        entity.Path = insert.SavedBookName;

        await HandleImage(entity, insert);
    }

    protected override async Task AfterInsert(EBook entity, EBookInsertRequest insert)
    {
        await HandleChapters(entity, insert);

        await Context.Entry(entity).Reference(e => e.Genre).LoadAsync();
        await Context.Entry(entity).Reference(e => e.MaturityRating).LoadAsync();
    }

    public async Task<ActionResult<string>> Save(IFormFile file)
    {
        return new CreatedAtActionResult(null, null, null, await EBookFileHelper.SaveFile(file));
    }
    
    public async Task<ActionResult<EBookParseDto>> Parse(IFormFile file)
    {
        if (!await EBookFileHelper.IsValidEpub(file))
        {
            throw new AppException("Invalid EPUB");
        }

        var data = await EBookFileHelper.ParseEpub(file);

        return new OkObjectResult(data);
    }
    
    private async Task ValidateForeignKeys(EBookInsertRequest insert)
    {
        var authorExists = await Context.Users.AnyAsync(user => user.Id == insert.AuthorId);
        var genreExists = await Context.Genres.AnyAsync(genre => genre.Id == insert.GenreId);
        var maturityRatingExists =
            await Context.MaturityRatings.AnyAsync(rating => rating.Id == insert.MaturityRatingId);

        if (!authorExists) throw new AppException("The author for this eBook does not exist!");
        if (!genreExists) throw new AppException("The genre for this eBook was not found!");
        if (!maturityRatingExists) throw new AppException("The maturity rating for this eBook was not found!");
    }

    private void ValidateIfSaved(EBookInsertRequest insert)
    {
        if (!EBookFileHelper.Saved(insert.SavedBookName))
        {
            throw new AppException("The eBook has not been saved to disk!");
        }
    }

    private async Task HandleImage(EBook entity, EBookInsertRequest insert)
    {
        var savePath = Path.Combine("images", "ebooks",
            $"{entity.Path.Split('.')[0]}");
        var imageSaveInfo = ImageHelper.SaveFromBase64(insert.ParsedInfo.EncodedCoverArt, null, savePath);
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
        
        for (var i = 0; i < insert.ParsedInfo.Chapters.Count; i++)
        {
            var newChapter = new EBookChapter()
            {
                ChapterName = insert.ParsedInfo.Chapters[i],
                ChapterNumber = i,
                EBookId = entity.Id
            };
            
            chapters.Add(newChapter);
        }

        await Context.EBookChapters.AddRangeAsync(chapters);
        entity.ChapterCount = chapters.Count;
        await Context.SaveChangesAsync();
    }
}