using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.SeedObjects;
using Wordsmith.Utils;

namespace Wordsmith.DataAccess.Db.Seeds;

public static class DatabaseSeeds
{
    private static string _webRootPath;
    private static string _ebookImagesPath;
    private static string _ebookFilepath;
    private static string _seedListsPath;
    
    private const string DefaultAuthorUsername = "jane_doe2";

    public static void Init(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _webRootPath = environment.WebRootPath;
        _seedListsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedLists");
        _ebookImagesPath = Path.Combine("images", "ebooks");
        _ebookFilepath = configuration["EBookSettings:SavePath"];
    }
    
    public static void EnsureSeedData(DatabaseContext context)
    {
        Logger.LogInfo("Checking whether seeding is necessary...");

        CreateUsers(context);
        CreateMaturityRatings(context);
        CreateGenres(context);
        CreateEbooks(context);
        
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
                Username = DefaultAuthorUsername,
                Email = $"{DefaultAuthorUsername}@personal.com",
                PayPalEmail = $"{DefaultAuthorUsername}@personal.com",
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

    private static void CreateEbooks(DatabaseContext context)
    {
        var booksPath = Path.Combine(_seedListsPath, "Books");
        var bookListPath = Path.Combine(booksPath, "books.json");
        var author = context.Users.FirstOrDefault(e => e.Username == DefaultAuthorUsername);

        if (!File.Exists(bookListPath))
        {
            Logger.LogWarn("The seed list for books does not exist, skipping seeding");
            return;
        }

        if (author == null)
        {
            Logger.LogWarn("The author for the books to be seeded does not exist, skipping seeding");
            return;
        }

        var bookSeeds = ParseSeedList<BookSeed>(bookListPath);
        var seedCount = 0;

        foreach (var seed in bookSeeds)
        {
            if (context.EBooks.Any(e => e.Title == seed.Title)) continue;

            var pathToEpub = Path.Combine(booksPath, seed.BookFilename);
            var pathToCoverArt = Path.Combine(booksPath, seed.ImageFilename);
            
            if (!File.Exists(pathToEpub))
            {
                Logger.LogWarn($"Referenced book seed filename {seed.BookFilename} was not found, skipping...");
                continue;
            }

            if (!File.Exists(pathToCoverArt))
            {
                Logger.LogWarn($"Referenced book seed cover art {seed.ImageFilename} was not found, skipping...");
                continue;
            }

            var image = CreateImage(pathToCoverArt, seed.ImageFilename, context);
            
            var copiedPathToEpub = Path.Combine(_ebookFilepath, seed.BookFilename);
            File.Copy(pathToEpub, copiedPathToEpub, true);

            var random = new Random();
            var randomNumberOfDays = random.Next(-365, 0);
            var bookDate = DateTime.UtcNow.AddDays(randomNumberOfDays);
            var ebook = new EBook()
            {
                Title = seed.Title,
                Description = seed.Description,
                RatingAverage = seed.RatingAverage,
                Author = author,
                Price = seed.Price,
                ChapterCount = seed.ChapterCount,
                Path = seed.BookFilename,
                Genres = "",
                MaturityRating = context.MaturityRatings.First(e => e.ShortName == seed.MaturityRating),
                PublishedDate = bookDate,
                UpdatedDate = bookDate,
                IsHidden = false,
                CoverArt = image,
            };
            
            CreateChapters(ebook, context);
            CreateEbookGenres(ebook, context, seed.Genres);
            
            context.EBooks.Add(ebook);
            seedCount++;
        }
        
        if (seedCount != 0) Logger.LogInfo($"Seeded {seedCount} new ebooks");
    }

    private static void CreateChapters(EBook ebook, DatabaseContext context)
    {
        for (var i = 1; i <= ebook.ChapterCount; i++)
        {
            context.EBookChapters.Add(new EBookChapter()
            {
                ChapterName = $"Chapter {i}",
                ChapterNumber = i,
                EBook = ebook
            });
        }
    }

    private static void CreateEbookGenres(EBook ebook, DatabaseContext context, string genres)
    {
        var genreArray = genres.Split(";");

        foreach (var genreName in genreArray)
        {
            var genre = context.Genres.FirstOrDefault(e => e.Name == genreName);

            if (genre == null)
            {
                Logger.LogWarn($"Genre {genreName} for ebook seed was not found, skipping it");
                continue;
            }
            
            context.EBookGenres.Add(new EBookGenre()
            {
                EBook = ebook,
                Genre = genre,
            });

            ebook.Genres += $"{genre.Name};";
        }
    }

    private static Image CreateImage(string imageFilepath, string imageFilename, DatabaseContext context)
    {
        var existingImage = context.Images.FirstOrDefault(e => e.Path == Path.Combine(_ebookImagesPath, imageFilename));
        
        if (existingImage != null) return existingImage;
        
        var newFilepath = Path.Combine(_webRootPath, _ebookImagesPath, imageFilename);
        File.Copy(imageFilepath, newFilepath, true);

        var image = new Image()
        {
            Format = Path.GetExtension(newFilepath).Replace(".", ""),
            Size = (int)new FileInfo(newFilepath).Length, // Please don't overflow
            Path = Path.Combine(_ebookImagesPath, imageFilename)
        };
        
        context.Images.Add(image);

        return image;
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
    
    private static IEnumerable<T> ParseSeedList<T>(string path) where T : class
    {
        var jsonString = File.ReadAllText(path);
        return JsonSerializer.Deserialize<IEnumerable<T>>(jsonString);
    }
}