using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects.EBookRating;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.EBookRating;

public class EBookRatingService : WriteService<EBookRatingDto, Db.Entities.EBookRating, EBookRatingSearchObject,
    EBookRatingInsertRequest, EBookRatingUpdateRequest>, IEBookRatingService
{
    public EBookRatingService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }

    protected override async Task BeforeInsert(Db.Entities.EBookRating entity, EBookRatingInsertRequest insert)
    {
        await ValidateInsertion(insert.EBookId, insert.UserId);
        entity.RatingDate = DateTime.UtcNow;
    }

    protected override async Task AfterInsert(Db.Entities.EBookRating entity, EBookRatingInsertRequest insert)
    {
        await CalculateRatingAverage(entity);
    }

    protected override IQueryable<Db.Entities.EBookRating> AddFilter(IQueryable<Db.Entities.EBookRating> query, EBookRatingSearchObject search)
    {
        if (search.Rating.HasValue)
        {
            query = query.Where(e => e.Rating == search.Rating.Value);
        }

        if (search.EBookId.HasValue)
        {
            query = query.Where(e => e.EBookId == search.EBookId.Value);
        }

        if (search.UserId.HasValue)
        {
            query = query.Where(e => e.UserId == search.UserId);
        }

        return query;
    }

    private async Task ValidateInsertion(int eBookId, int userId)
    {
        if (!await Context.EBooks.AnyAsync(e => e.Id == eBookId))
        {
            throw new AppException("The ebook you are trying to rate does not exist!");
        }

        if (!await Context.Users.AnyAsync(e => e.Id == userId))
        {
            throw new AppException("The user that is trying to rate does not exist!");
        }

        if (!await Context.UserLibraries.AnyAsync(e => e.UserId == userId && e.EBookId == eBookId))
        {
            throw new AppException("You cannot rate an ebook you do not have you in your library!");
        }

        if (await Context.EBookRatings.AnyAsync(e => e.EBookId == eBookId && e.UserId == userId))
        {
            throw new AppException("You have already rated this ebook!");
        }
    }

    private async Task CalculateRatingAverage(Db.Entities.EBookRating rating)
    {
        var ebook = await Context.EBooks.FirstAsync(e => e.Id == rating.EBookId);
        var newRatingAverage = await Context.EBookRatings.Where(e => e.EBookId == rating.EBookId).AverageAsync(e => e.Rating);
        
        ebook.RatingAverage = Math.Round(newRatingAverage, 2);
        await Context.SaveChangesAsync();
    }
}