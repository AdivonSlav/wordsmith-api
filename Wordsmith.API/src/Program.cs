using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Wordsmith.API.Middleware;
using Wordsmith.DataAccess.AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;

#pragma warning disable IDE0058

try
{
    var builder = WebApplication.CreateBuilder(args);
    Logger.Init(builder.Configuration["Logging:NLog:LogLevel"] ?? "Debug");

    // Add implementations for services so they can be dependency injected
    builder.Services.AddTransient<GlobalExceptionHandler>();
    builder.Services
        .AddTransient<IReadService<ReportReasonDto, SearchObject>, ReadService<ReportReasonDto,
            ReportReason, SearchObject>>();
    builder.Services
        .AddTransient<IUserService, UserService>();

    var webRootPath = builder.Environment.WebRootPath;
    var imageSettings = builder.Configuration.GetSection("ImageSettings");
    ImageHelper.Init(webRootPath, imageSettings["AllowedSize"], imageSettings["AllowedFormats"]);

    // Ensure that URLs are auto-created as lowercase 
    builder.Services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
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
    });
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = "https://localhost:7443";
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false
            };
        });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("ApiScope", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", "wordsmith_api.read", "wordsmith_api.write", "wordsmith_api.full_access");
        });
    });

    var mysqlDetails = builder.Configuration.GetSection("Connection").GetSection("MySQL");
    var mysqlConnectionString = $"Server={mysqlDetails["Host"]};" +
                                $"Port={mysqlDetails["Port"]};" +
                                $"Uid={mysqlDetails["User"]};" +
                                $"Pwd={mysqlDetails["Password"]};" +
                                $"Database={mysqlDetails["Database"]};";
    var mysqlVersion = ServerVersion.AutoDetect(mysqlConnectionString);
    
    builder.Services.AddDbContext<DatabaseContext>(options =>
    {
        options.UseMySql(mysqlConnectionString, mysqlVersion,
            optionsBuilder => { optionsBuilder.MigrationsAssembly("Wordsmith.DataAccess"); });
    });
    
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<GlobalExceptionHandler>();

    app.MapControllers();

    Logger.LogInfo("Listening...");
    app.Run();
}
catch (Exception e)
{
    Logger.LogFatal("API bootstrapping failed due to an exception", e);
}
finally
{
    Logger.Cleanup();
}
