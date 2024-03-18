using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Db;

public static class DatabaseSeeds
{
    public static void EnsureSeedData(DatabaseContext context)
    {
        Logger.LogInfo("Checking whether seeding is necessary...");

        CreateUsers(context);
        CreateMaturityRatings(context);
        CreateGenres(context);
        
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
    
    private static void CreateMaturityRatings(DatabaseContext context)
    {
        var ratings = GetMaturityRatings();
        var hasSeeded = false;
        
        foreach (var rating in ratings)
        {
            var ratingName = rating.Split(";")[0];
            var ratingShortName = rating.Split(";")[1];

            if (context.MaturityRatings.Any(e => e.Name == ratingName && e.ShortName == ratingShortName)) continue;

            context.MaturityRatings.Add(new MaturityRating() { Name = ratingName, ShortName = ratingShortName });
            hasSeeded = true;
        }
        
        if (hasSeeded) Logger.LogInfo("Seeded new maturity ratings");
    }
    
    private static void CreateGenres(DatabaseContext context)
    {
        var genres = GetGenres();
        var hasSeeded = false;

        foreach (var genre in genres)
        {
            if (context.Genres.Any(e => e.Name == genre)) continue;

            context.Genres.Add(new Genre() { Name = genre });
            hasSeeded = true;
        }
        
        if (hasSeeded) Logger.LogInfo("Seeded new genres");
    }
    
    private static IEnumerable<string> GetMaturityRatings()
    {
        return new List<string>()
        {
            "Kids;K",
            "Teens;T",
            "Mature;M"
        };
    }

    private static IEnumerable<string> GetGenres()
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