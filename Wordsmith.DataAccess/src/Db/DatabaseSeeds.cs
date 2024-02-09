using Microsoft.Extensions.Configuration;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Db;

public static class DatabaseSeeds
{
    public static void CreateUsers(DatabaseContext context, IConfiguration configuration)
    {
        if (context.Users.Any()) return;
        
        var adminToCreate = configuration["DefaultAdmin"];
        var userToCreate = configuration["DefaultUser"];
        var createdUsers = new List<User>();

        if (adminToCreate != null)
        {
            createdUsers.Add(new User
            {
                Id = 1,
                Username = adminToCreate,
                Email = $"{adminToCreate}@example.com",
                RegistrationDate = DateTime.UtcNow,
                About = "Just an admin",
                Role = "admin",
            });
        }
        
        if (userToCreate != null)
        {
            createdUsers.Add(new User
            {
                Id = 2,
                Username = userToCreate,
                Email = $"{userToCreate}@example.com",
                RegistrationDate = DateTime.UtcNow,
                About = "Just a user",
                Role = "user",
            });
        }
        
        context.Users.AddRange(createdUsers);
        context.SaveChanges();
        Logger.LogInfo("Created default users!");
    }
    
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