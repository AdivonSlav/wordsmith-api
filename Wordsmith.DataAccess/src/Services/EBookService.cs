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
        
        entity.PublishedDate = DateTime.UtcNow;
        entity.UpdatedDate = entity.PublishedDate;

        var fileName = await EBookFileHelper.SaveFile(insert.File);
        entity.Path = fileName;

        var savePath = Path.Combine("images", "ebooks",
            $"{entity.Title}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.{insert.CoverArt.Format}");
        var imageSaveInfo = ImageHelper.SaveFromBase64(insert.CoverArt.EncodedImage, insert.CoverArt.Format, savePath);
        var newImage = new Image()
        {
            Format = imageSaveInfo.Format,
            Size = imageSaveInfo.Size,
            Path = savePath
        };

        await Context.Images.AddAsync(newImage);
        entity.CoverArt = newImage;
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

}