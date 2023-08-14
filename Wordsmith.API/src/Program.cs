using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.AutoMapper;
using Wordsmith.DataAccess.DB;

#pragma warning disable IDE0058

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), optionsBuilder =>
    {
        optionsBuilder.MigrationsAssembly("Wordsmith.DataAccess");
    });
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