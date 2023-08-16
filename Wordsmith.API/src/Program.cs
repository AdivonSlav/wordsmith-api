using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.AutoMapper;
using Wordsmith.DataAccess.DB;
using Wordsmith.DataAccess.DB.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models;
using Wordsmith.Models.SearchObjects;

#pragma warning disable IDE0058

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add implementations for services so they can be dependency injected
builder.Services
    .AddTransient<IReadService<ReportReasonDTO, SearchObject>, ReadService<ReportReasonDTO,
        ReportReason, SearchObject>>();


// Ensure that URLs are auto-created as lowercase 
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        optionsBuilder => { optionsBuilder.MigrationsAssembly("Wordsmith.DataAccess"); });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();