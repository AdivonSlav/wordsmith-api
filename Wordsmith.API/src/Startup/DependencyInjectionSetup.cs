using System.Reflection;
using MerriamWebster.NET;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Wordsmith.API.Middleware;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Services.AppReport;
using Wordsmith.DataAccess.Services.Comment;
using Wordsmith.DataAccess.Services.EBook;
using Wordsmith.DataAccess.Services.EBookChapter;
using Wordsmith.DataAccess.Services.EBookRating;
using Wordsmith.DataAccess.Services.EBookReport;
using Wordsmith.DataAccess.Services.Genre;
using Wordsmith.DataAccess.Services.MaturityRating;
using Wordsmith.DataAccess.Services.Order;
using Wordsmith.DataAccess.Services.ReportReason;
using Wordsmith.DataAccess.Services.User;
using Wordsmith.DataAccess.Services.UserLibrary;
using Wordsmith.DataAccess.Services.UserLibraryCategory;
using Wordsmith.DataAccess.Services.UserReport;
using Wordsmith.Integration.MerriamWebster;
using Wordsmith.Integration.Paypal;
using Wordsmith.Utils.LoginClient;
using Wordsmith.Utils.ProfanityDetector;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.API.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var mysqlDetails = configuration.GetSection("Connection:MySQL");
        var mysqlConnectionString = $"Server={mysqlDetails["Host"]};" +
                                    $"Port={mysqlDetails["Port"]};" +
                                    $"Uid={mysqlDetails["User"]};" +
                                    $"Pwd={mysqlDetails["Password"]};" +
                                    $"Database={mysqlDetails["Database"]};";
        var mysqlVersion = ServerVersion.AutoDetect(mysqlConnectionString);
        var migrationsAssembly = typeof(DatabaseContext).Assembly.GetName().Name;
        
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseMySql(mysqlConnectionString, mysqlVersion,
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(migrationsAssembly);
                    optionsBuilder.EnableStringComparisonTranslations();
                });
        });
        
        services.AddAutoMapper(config =>
        {
            var dataAccessAssembly = Assembly.GetAssembly(typeof(DatabaseContext));
            config.AddMaps(dataAccessAssembly);
        });

        return services;
    }

    public static IServiceCollection RegisterStandardServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<GlobalExceptionHandler>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IReportReasonService, ReportReasonService>();
        services.AddTransient<IUserReportService, UserReportService>();
        services.AddTransient<IEBookService, EBookService>();
        services.AddTransient<IEBookReportService, EBookReportService>();
        services.AddTransient<IMaturityRatingService, MaturityRatingService>();
        services.AddTransient<IGenreService, GenreService>();
        services.AddTransient<IUserLibraryService, UserLibraryService>();
        services.AddTransient<IUserLibraryCategoryService, UserLibraryCategoryService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IEBookRatingService, EBookRatingService>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<IEBookChapterService, EBookChapterService>();
        services.AddTransient<IAppReportService, AppReportService>();

        services.AddScoped<IMerriamWebsterService, MerriamWebsterService>();
        services.AddScoped<IProfanityDetector, ProfanityDetector>();
        services.AddScoped<IMessageProducer, MessageProducer>();
        services.AddScoped<IMessageListener, MessageListener>();
        services.AddScoped<ILoginClient, LoginClient>(provider =>
        {
            var clientFactory = provider.GetService<IHttpClientFactory>();
            var identityServerHost = configuration["Connection:IdentityServer:Host"];
            var identityServerPort = configuration["Connection:IdentityServer:Port"];

            if (string.IsNullOrWhiteSpace(identityServerHost) || string.IsNullOrWhiteSpace(identityServerPort))
                throw new Exception("IdentityServer not configured!");

            var identityServerAddress = $"https://{identityServerHost}:{identityServerPort}";

            return new LoginClient(identityServerAddress, clientFactory!);
        });
        services.AddScoped<IPaypalService, PaypalService>(provider =>
        {
            var clientFactory = provider.GetService<IHttpClientFactory>();
            var clientId = configuration["PayPalClientId"];
            var clientSecret = configuration["PayPalClientSecret"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new Exception("PayPal clientId or clientSecret not configured!");
            }

            return new PaypalService(clientId, clientSecret, clientFactory!);
        });
        
        // Registers the MerriamWebster.NET package for DI
        services.RegisterMerriamWebster(configuration.GetSection("MerriamWebster").Get<MerriamWebsterConfig>());
        
        // Registers the HttpClientFactory to be used for managing client instances
        services.AddHttpClient();

        // Ensure that URLs are auto-created as lowercase 
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection RegisterAuthServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            var jwtScheme = new OpenApiSecurityScheme()
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your bearer token in the textbox below!",
                Reference = new OpenApiReference()
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { jwtScheme, Array.Empty<string>() }
            });
            
            options.EnableAnnotations();
        });

        // Authentication is set up for JWT bearer tokens and the authority is the IdentityServer at the configured address
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority =
                    $"https://{configuration["Connection:IdentityServer:Host"]}:{configuration["Connection:IdentityServer:Port"]}";
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("All", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "wordsmith_api.read", "wordsmith_api.write", "wordsmith_api.full_access");
            });
            options.AddPolicy("AdminOperations", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "wordsmith_api.full_access");
            });
            options.AddPolicy("UserOperations", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "wordsmith_api.read", "wordsmith_api.write");
            });
        });

        return services;
    }
}