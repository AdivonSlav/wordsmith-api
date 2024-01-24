using Wordsmith.DataAccess.Db.Entities;

namespace Wordsmith.DataAccess.Db;

public static class DatabaseSeeds
{
    public static IEnumerable<MaturityRating> CreateMaturityRatings()
    {
        var ratings = GetMaturityRatings();

        return ratings.Select((rating, i) => new MaturityRating() { Id = i + 1, Name = rating.Split(";")[0], ShortName = rating.Split(";")[1] }).ToList();
    }
    
    public static IEnumerable<Genre> CreateGenres()
    {
        var genres = GetGenres();

        return genres.Select((genre, i) => new Genre() { Id = i + 1, Name = genre }).ToList();
    }
    
    private static List<string> GetMaturityRatings()
    {
        return new List<string>()
        {
            "Kids;K",
            "Teens;T",
            "Mature;M"
        };
    }

    private static List<string> GetGenres()
    {
        return new List<string>()
        {
            "Fiction",
            "Mystery",
            "Thriller",
            "Science Fiction",
            "Fantasy",
            "Romance",
            "Historical Fiction",
            "Horror",
            "Adventure",
            "Crime",
            "Comedy",
            "Drama",
            "Non-Fiction",
            "Biography",
            "Autobiography",
            "Memoir",
            "Self-Help",
            "Philosophy",
            "Psychology",
            "Science",
            "Technology",
            "Business",
            "Economics",
            "History",
            "Politics",
            "Sociology",
            "Travel",
            "Poetry",
            "Anthology",
            "Children's",
            "Young Adult (YA)",
            "Middle Grade",
            "Graphic Novel",
            "Comic Book",
            "Satire",
            "Dystopian",
            "Utopian",
            "Paranormal",
            "Supernatural",
            "Historical Romance",
        };
    }
}