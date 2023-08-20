using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
    Logger.Init();
    var builder = WebApplication.CreateBuilder(args);

    // Add implementations for services so they can be dependency injected
    builder.Services
        .AddTransient<IReadService<ReportReasonDto, SearchObject>, ReadService<ReportReasonDto,
            ReportReason, SearchObject>>();


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

    var connectionString = builder.Configuration.GetConnectionString("Default");
    builder.Services.AddDbContext<DatabaseContext>(options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
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

    app.MapControllers().RequireAuthorization("ApiScope");

    Logger.LogInfo("Listening...");
    app.Run();
}
catch (Exception e)
{
    Logger.LogFatal("API bootstrapping failed due to unrecoverable errors", e);
    throw;
}
finally
{
    Logger.Cleanup();
}
