using System.Text.Json;
using CrypticWizard.RandomWordGenerator;
using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Enums;
using Wordsmith.Models.SeedObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.EBookFileHelper;

namespace Wordsmith.DataAccess.Db.Seeds;

internal class DateTimeRange
{
    public DateTime Start { get; }
    public DateTime End { get; }
    
    public DateTimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }
}

public static class DatabaseSeeds
{
    private static readonly Random Rand = new();
    private static readonly WordGenerator WordGenerator = new();
    
    private static readonly string SeedDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData");

    private const string DefaultAdminUsername = "orwell47";
    private const string DefaultUserUsername = "john_doe1";
    private const string DefaultAuthorUsername = "jane_doe2";
    
    private static readonly List<DateTimeRange> SeedDateClusters = new()
    {
        new DateTimeRange(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddYears(-1).AddMonths(3)), // Cluster 1: 1 year ago to 9 months ago
        new DateTimeRange(DateTime.UtcNow.AddYears(-1).AddMonths(6), DateTime.UtcNow.AddMonths(-6)), // Cluster 2: 6 months ago to 6 months ago
        new DateTimeRange(DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow.AddMonths(-1)), // Cluster 3: 3 months ago to 1 month ago
        new DateTimeRange(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow), // Cluster 4: 30 days ago to now
    };

    public static async Task EnsureSeedData(DatabaseContext context)
    {
        Logger.LogInfo("Checking whether seeding is necessary...");

        await CreateUsers(context);
        await CreateMaturityRatings(context);
        await CreateGenres(context);
        await CreateReportReasons(context);
        await context.SaveChangesAsync();
        
        await CreateUserReports(context);
        await CreateAppReports(context);
        await context.SaveChangesAsync();
        
        await CreateEbooks(context);
        await context.SaveChangesAsync();
        
        await CreateComments(context);
        await CreateEbookReports(context);
        await CreateUserLibraries(context);
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
                Username = DefaultAdminUsername,
                Email = "orwell47@personal.com",
                PayPalEmail = "orwell47@personal.com", 
                Role = "admin",
                RegistrationDate = DateTime.UtcNow,
                About = "Just an admin",
            },
            new()
            {
                Id = 2,
                Username = DefaultUserUsername,
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

        // Create accessible users
        foreach (var user in users)
        {
            if (!await context.Users.AnyAsync(e => e.Id == user.Id && e.Username == user.Username))
            {
                await context.Users.AddAsync(user);
            }    
        }
        
        // Create inaccessible users (starting from ID4)
        for (var i = 4; i < 104; i++)
        {
            var username = GenerateUsername();
            
            if (await context.Users.AnyAsync(e => e.Username == username)) continue;
            
            await context.Users.AddAsync(new User()
            {
                Id = i,
                Username = username,
                Email = $"{username}@inaccessible.com",
                PayPalEmail = $"{username}@inaccessible.com",
                Role = "user",
                RegistrationDate = GenerateRandomDate(),
                About = "Inaccessible user",
                Status = UserStatus.Banned
            });
        }
        
        Logger.LogInfo("Seeded users to the database");
    }
    
    private static async Task CreateMaturityRatings(DatabaseContext context)
    {
        if (await context.MaturityRatings.AnyAsync()) return;
        
        var ratings = GetMaturityRatings();
        
        foreach (var rating in ratings)
        {
            var ratingName = rating.Split(";")[0];
            var ratingShortName = rating.Split(";")[1];

            await context.MaturityRatings.AddAsync(new MaturityRating() { Name = ratingName, ShortName = ratingShortName });
        }
        
        Logger.LogInfo("Seeded new maturity ratings");
    }
    
    private static async Task CreateGenres(DatabaseContext context)
    {
        if (await context.Genres.AnyAsync()) return;
        
        var genres = GetGenres();

        foreach (var genre in genres)
        {
            await context.Genres.AddAsync(new Genre() { Name = genre });
        }
        
        Logger.LogInfo("Seeded new genres");
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
            
            var randomDate = GenerateRandomDate();
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
                PublishedDate = randomDate,
                UpdatedDate = randomDate,
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

    private static async Task CreateReportReasons(DatabaseContext context)
    {
        if (await context.ReportReasons.AnyAsync()) return;
        
        var userReportReasons = GetUserReportReasons();
        var ebookReportReasons = GetEbookReportReasons();

        foreach (var reason in userReportReasons)
        {
            await context.ReportReasons.AddAsync(new ReportReason()
            {
                Reason = reason,
                Type = ReportType.User
            });
        }
        
        foreach (var reason in ebookReportReasons)
        {
            await context.ReportReasons.AddAsync(new ReportReason()
            {
                Reason = reason,
                Type = ReportType.Ebook
            });
        }
        
        Logger.LogInfo("Seeded new report reasons");
    }

    private static async Task CreateUserReports(DatabaseContext context)
    {
        if (await context.UserReports.AnyAsync()) return;

        var seller = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultAuthorUsername);
        var user = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultUserUsername);

        if (seller == null)
        {
            Logger.LogWarn("Seeded seller account needed for user report seeding not found, skipping seeding");
            return;
        }

        if (user == null)
        {
            Logger.LogWarn("Seeded user account needed for user report seeding not found, skipping seeding");
            return;
        }

        var reportDetails = await context.ReportDetails.AddAsync(new ReportDetails()
        {
            Reporter = seller,
            ReportReason = await context.ReportReasons.SingleAsync(e => e.Reason == "Inappropriate content" && e.Type == ReportType.User),
            SubmissionDate = DateTime.UtcNow,
            Content = "I am writing to report an incident regarding inappropriate content uploaded by this user on your platform. The user in question has posted rude comments!",
        });
        await context.UserReports.AddAsync(new UserReport()
        {
            ReportDetails = reportDetails.Entity,
            ReportedUser = user,
        });
        
        reportDetails = await context.ReportDetails.AddAsync(new ReportDetails()
        {
            Reporter = seller,
            ReportReason = await context.ReportReasons.SingleAsync(e => e.Reason == "Spam" && e.Type == ReportType.User),
            SubmissionDate = DateTime.UtcNow,
            Content = @"The user in question has posted several spam comments on my ebooks. Urge you to please check this out and do something about it!",
        });
        await context.UserReports.AddAsync(new UserReport()
        {
            ReportDetails = reportDetails.Entity,
            ReportedUser = user,
        });
        
        Logger.LogInfo("Seeded new user reports");
    }
    
    private static async Task CreateEbookReports(DatabaseContext context)
    {
        if (await context.EBookReports.AnyAsync()) return;

        var author = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultAuthorUsername);
        var user = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultUserUsername);
        
        if (author == null)
        {
            Logger.LogWarn("Seeded author account needed for ebook report seeding not found, skipping seeding");
            return;
        }

        if (user == null)
        {
            Logger.LogWarn("Seeded user account needed for ebook report seeding not found, skipping seeding");
            return;
        }
        
        if (!await context.EBooks.AnyAsync(e => e.AuthorId == author.Id))
        {
            Logger.LogWarn("No ebooks authored by the default seed author found in database, skipping seeding of ebook reports");
            return;
        }

        var reportDetails = await context.ReportDetails.AddAsync(new ReportDetails()
        {
            Reporter = user,
            ReportReason = await context.ReportReasons.SingleAsync(e => e.Reason == "Low quality or errors" && e.Type == ReportType.Ebook),
            SubmissionDate = DateTime.UtcNow,
            Content = "This book is of low quality. You should have better standards for vetting newly published books on this platform",
        });
        await context.EBookReports.AddAsync(new EBookReport()
        {
            ReportDetails = reportDetails.Entity,
            ReportedEBook = await context.EBooks.FirstAsync()
        });
        
        reportDetails = await context.ReportDetails.AddAsync(new ReportDetails()
        {
            Reporter = user,
            ReportReason = await context.ReportReasons.SingleAsync(e => e.Reason == "Plagiarism" && e.Type == ReportType.Ebook),
            SubmissionDate = DateTime.UtcNow,
            Content = "This book is obviously a plagiarism of a popular book from the 19th century. Please take it down!",
        });
        await context.EBookReports.AddAsync(new EBookReport()
        {
            ReportDetails = reportDetails.Entity,
            ReportedEBook = await context.EBooks.FirstAsync()
        });
        
        Logger.LogInfo("Seeded new ebook reports");
    }
    
    private static async Task CreateAppReports(DatabaseContext context)
    {
        if (await context.AppReports.AnyAsync()) return;

        var user = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultUserUsername);

        if (user == null)
        {
            Logger.LogWarn("Seeded user account needed for app report seeding not found, skipping seeding");
            return;
        }

        await context.AppReports.AddAsync(new AppReport()
        {
            Content = "I want to be able to use other payment methods and not just PayPal!",
            SubmissionDate = DateTime.UtcNow,
            User = user,
        });
        await context.AppReports.AddAsync(new AppReport()
        {
            Content = "The built-in app reader is very laggy!",
            SubmissionDate = DateTime.UtcNow,
            User = user,
        });
        
        Logger.LogInfo("Seeded new app reports");
    }

    private static async Task CreateComments(DatabaseContext context)
    {
        if (await context.Comments.AnyAsync()) return;
        
        var author = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultAuthorUsername);
        var user = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultUserUsername);
        
        if (author == null)
        {
            Logger.LogWarn("Seeded author account needed for comment seeding not found, skipping seeding");
        }
        
        if (user == null)
        {
            Logger.LogWarn("Seeded user account needed for comment seeding not found, skipping seeding");
        }
        
        if (!await context.EBooks.AnyAsync(e => e.AuthorId == author.Id))
        {
            Logger.LogWarn("No ebooks authored by the default seed author found in database, skipping seeding of comments");
            return;
        }

        var ebooks = await context.EBooks.Where(e => e.AuthorId == author.Id).ToListAsync();

        foreach (var ebook in ebooks)
        {
            await context.Comments.AddAsync(new Comment()
            {
                DateAdded = DateTime.UtcNow,
                Content = "Great book! Highly recommended",
                EBook = ebook,
                User = user,
            });
        }
        
        Logger.LogInfo("Seeded new comments");
    }

    private static async Task CreateUserLibraries(DatabaseContext context)
    {
        if (await context.UserLibraries.AnyAsync()) return;
        
        var author = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultAuthorUsername);
        var user = await context.Users.FirstOrDefaultAsync(e => e.Username == DefaultUserUsername);
        
        if (author == null)
        {
            Logger.LogWarn("Seeded author account needed for library seeding not found, skipping seeding");
            return;
        }
        
        if (user == null)
        {
            Logger.LogWarn("Seeded user account needed for library seeding not found, skipping seeding");
            return;
        }
        
        if (!await context.EBooks.AnyAsync(e => e.AuthorId == author.Id))
        {
            Logger.LogWarn("No ebooks authored by the default seed author found in database, skipping seeding of user libraries");
            return;
        }

        // Create library entry for the accessible user
        var ebooks = await context.EBooks
            .Include(e => e.Author)
            .Where(e => e.Price != null)
            .Where(e => e.Author == author)
            .ToListAsync();
        var ebookForAccessibleUser = ebooks.First();
        var dateNow = DateTime.UtcNow;
        await context.UserLibraries.AddAsync(new UserLibrary()
        {
           EBook = ebookForAccessibleUser,
           User = user,
           SyncDate = dateNow,
        });
        await context.UserLibraryHistories.AddAsync(new UserLibraryHistory()
        {
            EBook = ebookForAccessibleUser,
            SyncDate = dateNow,
            User = user,
        });
        await context.Orders.AddAsync(new Order()
        {
            ReferenceId = Guid.NewGuid().ToString(),
            PayPalOrderId = Guid.NewGuid().ToString(),
            PayPalCaptureId = Guid.NewGuid().ToString(),
            OrderCreationDate = dateNow,
            PaymentDate = dateNow,
            Status = OrderStatus.Completed,
            Payee = author,
            PayeeUsername = author.Username,
            PayeePayPalEmail = author.PayPalEmail,
            Payer = user,
            PayerUsername = user.Username,
            EBook = ebookForAccessibleUser,
            EBookTitle = ebookForAccessibleUser.Title,
            PaymentUrl = "https://www.test.com",
            PaymentAmount = ebookForAccessibleUser.Price!.Value,
        });
        
        // Get first 20 inaccessible users
        const int inaccessibleToGet = 20;
        var inaccessibleUsers = await context.Users
            .Where(e => e.Status == UserStatus.Banned)
            .OrderBy(e => e.Id)
            .Take(inaccessibleToGet)
            .ToListAsync();
        // Create library entries for the inaccessible users
        foreach (var ebook in ebooks)
        {
            var randomSubset = inaccessibleUsers.Take(Rand.Next(1, inaccessibleUsers.Count));
            
            foreach (var inaccessibleUser in randomSubset)
            {
                if (await context.UserLibraries.AnyAsync(e => e.UserId == inaccessibleUser.Id && e.EBook == ebook))
                    continue;
                
                var randomDate = GenerateRandomDate();
                await context.UserLibraries.AddAsync(new UserLibrary()
                {
                    EBook = ebook,
                    User = inaccessibleUser,
                    SyncDate = randomDate,
                });
                await context.UserLibraryHistories.AddAsync(new UserLibraryHistory()
                {
                    EBook = ebook,
                    SyncDate = randomDate,
                    User = inaccessibleUser
                });
                if (ebook.Price != null)
                {
                    await context.Orders.AddAsync(new Order()
                    {
                        ReferenceId = Guid.NewGuid().ToString(),
                        PayPalOrderId = Guid.NewGuid().ToString(),
                        PayPalCaptureId = Guid.NewGuid().ToString(),
                        OrderCreationDate = randomDate,
                        PaymentDate = randomDate,
                        Status = OrderStatus.Completed,
                        Payee = author,
                        PayeeUsername = author.Username,
                        PayeePayPalEmail = author.PayPalEmail,
                        Payer = inaccessibleUser,
                        PayerUsername = inaccessibleUser.Username,
                        EBook = ebook,
                        EBookTitle = ebook.Title,
                        PaymentUrl = "https://www.test.com",
                        PaymentAmount = ebook.Price.Value,
                    });
                }
            }
        }
        
        Logger.LogInfo("Seeded new user libraries");
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

    private static IEnumerable<string> GetUserReportReasons()
    {
        return new List<string>()
        {
            "Inappropriate content",
            "Spam",
            "Harassment or bullying",
            "Misrepresentation"
        };
    }

    private static IEnumerable<string> GetEbookReportReasons()
    {
        return new List<string>()
        {
            "Copyright infringement",
            "Low quality or errors",
            "Plagiarism",
            "Inappropriate content"
        };
    }

    private static IEnumerable<T> ParseSeedList<T>(string path) where T : class
    {
        var jsonString = File.ReadAllText(path);
        return JsonSerializer.Deserialize<IEnumerable<T>>(jsonString);
    }
    
    private static string GenerateUsername()
    {
        const int usernameMaxLength = 20;
        
        var username = WordGenerator.GetWord(WordGenerator.PartOfSpeech.adj);
        username += Rand.Next(1000);
        
        // Trim if necessary
        if (username.Length > usernameMaxLength)
        {
            username = username[..usernameMaxLength];
        }
        
        return username;
    }
    
    private static DateTime GenerateRandomDate()
    {
        var randomCluster = SeedDateClusters[Rand.Next(SeedDateClusters.Count)];
        var range = randomCluster.End - randomCluster.Start;
        var randomTimeSpan = new TimeSpan((long)(Rand.NextDouble() * range.Ticks));
        
        return randomCluster.Start + randomTimeSpan;
    }
}