using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SeedObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.EBookFileHelper;

namespace Wordsmith.DataAccess.Db.Seeds;

public static class DatabaseSeeds
{
    private static readonly string SeedDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData");
    
    private const string DefaultAuthorUsername = "jane_doe2";

    public static async Task EnsureSeedData(DatabaseContext context)
    {
        Logger.LogInfo("Checking whether seeding is necessary...");

        await CreateUsers(context);
        await CreateMaturityRatings(context);
        await CreateGenres(context);
        
        await context.SaveChangesAsync();
        await CreateEbooks(context);
        await context.SaveChangesAsync();
    }
    
    private static async Task CreateUsers(DatabaseContext context)
    {
        if (await context.Users.AnyAsync())
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

        foreach (var user in users)
        {
            if (!await context.Users.AnyAsync(e => e.Id == user.Id && e.Username == user.Username))
            {
                await context.Users.AddAsync(user);
            }    
        }
        
        Logger.LogInfo("Seeded users to the database");
    }
    
    private static async Task CreateMaturityRatings(DatabaseContext context)
    {
        var ratings = GetMaturityRatings();
        var hasSeeded = false;
        
        foreach (var rating in ratings)
        {
            var ratingName = rating.Split(";")[0];
            var ratingShortName = rating.Split(";")[1];

            if (await context.MaturityRatings.AnyAsync(e => e.Name == ratingName && e.ShortName == ratingShortName)) continue;

            await context.MaturityRatings.AddAsync(new MaturityRating() { Name = ratingName, ShortName = ratingShortName });
            hasSeeded = true;
        }
        
        if (hasSeeded) Logger.LogInfo("Seeded new maturity ratings");
    }
    
    private static async Task CreateGenres(DatabaseContext context)
    {
        var genres = GetGenres();
        var hasSeeded = false;

        foreach (var genre in genres)
        {
            if (await context.Genres.AnyAsync(e => e.Name == genre)) continue;

            await context.Genres.AddAsync(new Genre() { Name = genre });
            hasSeeded = true;
        }
        
        if (hasSeeded) Logger.LogInfo("Seeded new genres");
    }

    private static async Task CreateEbooks(DatabaseContext context)
    {
        var bookListPath = Path.Combine(SeedDataPath, "books.json");
        var author = await context.Users.SingleOrDefaultAsync(e => e.Username == DefaultAuthorUsername);

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
            if (await context.EBooks.AnyAsync(e => e.Title == seed.Title)) continue;

            var pathToEpub = Path.Combine(SeedDataPath, seed.BookFilename);
            
            if (!File.Exists(pathToEpub))
            {
                Logger.LogWarn($"Referenced book seed filename {seed.BookFilename} was not found, skipping...");
                continue;
            }

            var parsedInfo = await EBookFileHelper.ParseEpub(pathToEpub);
            var image = await CreateEBookImage(parsedInfo, context);
            var newPathToEpub = EBookFileHelper.SaveFile(pathToEpub);

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
                ChapterCount = 0,
                Path = newPathToEpub,
                Genres = "",
                MaturityRating = await context.MaturityRatings.FirstAsync(e => e.ShortName == seed.MaturityRating),
                PublishedDate = bookDate,
                UpdatedDate = bookDate,
                IsHidden = false,
                CoverArt = image,
            };
            
            await CreateChapters(ebook, parsedInfo, context);
            await CreateEbookGenres(ebook, context, seed.Genres);
            
            await context.EBooks.AddAsync(ebook);
            seedCount++;
        }
        
        if (seedCount != 0) Logger.LogInfo($"Seeded {seedCount} new ebooks");
    }

    private static async Task CreateChapters(EBook ebook, EBookParseDto parsedInfo, DatabaseContext context)
    {
        var chapters = parsedInfo.Chapters.Select((t, i) => new EBookChapter() { ChapterName = t, ChapterNumber = i, EBook = ebook, }).ToList();

        await context.EBookChapters.AddRangeAsync(chapters);
        ebook.ChapterCount = chapters.Count;
    }

    private static async Task CreateEbookGenres(EBook ebook, DatabaseContext context, string genres)
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
            
            await context.EBookGenres.AddAsync(new EBookGenre()
            {
                EBook = ebook,
                Genre = genre,
            });

            ebook.Genres += $"{genre.Name};";
        }
    }

    private static async Task<Image> CreateEBookImage(EBookParseDto parsedInfo, DatabaseContext context)
    {
        var newFilepath = Path.Combine("images", "ebooks", $"eBook-{Guid.NewGuid()}");
        var saveInfo = ImageHelper.SaveFromBase64(parsedInfo.EncodedCoverArt, null, newFilepath);

        var image = new Image()
        {
            Format = saveInfo.Format,
            Size = saveInfo.Size,
            Path = saveInfo.Path
        };
        
        await context.Images.AddAsync(image);

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