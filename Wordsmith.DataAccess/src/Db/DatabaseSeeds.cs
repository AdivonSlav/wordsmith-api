using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Db;

public static class DatabaseSeeds
{
    public static void EnsureSeedData(DatabaseContext context)
    {
        Logger.LogInfo("Checking whether seeding is necessary...");

        CreateUsers(context);
        
        context.SaveChanges();
    }
    
    private static void CreateUsers(DatabaseContext context)
    {
        if (context.Users.Any())
        {
            return;
        }
        
        var users = new List<User>
        {
            new()
            {
                Id = 1,
                Username = "orwell47",
                Email = "orwell47@personal.com",
                PayPalEmail = "orwell47@personal.com", 
                Role = "admin",
                RegistrationDate = DateTime.UtcNow,
                About = "Just an admin",
            },
            new()
            {
                Id = 2,
                Username = "john_doe1",
                Email = "john_doe1@personal.com",
                PayPalEmail = "john_doe1@personal.com",
                Role = "user",
                RegistrationDate = DateTime.UtcNow,
                About = "Just a user",
            },
            new()
            {
                Id = 3,
                Username = "jane_doe2",
                Email = "jane_doe2@personal.com",
                PayPalEmail = "jane_doe2@personal.com",
                Role = "user",
                RegistrationDate = DateTime.UtcNow,
                About = "Just a seller",
            }
        };

        foreach (var user in users.Where(user => context.Users.FirstOrDefault(u => u.Username == user.Username && u.Id == user.Id) == null))
        {
            context.Users.Add(user);
        }
        
        Logger.LogInfo("Seeded users to the database");
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