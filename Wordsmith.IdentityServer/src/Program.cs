using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wordsmith.IdentityServer;
using Wordsmith.IdentityServer.Db;
using Wordsmith.IdentityServer.Db.Entities;

var builder = WebApplication.CreateBuilder(args);

var migrationsAssembly = typeof(Config).Assembly.GetName().Name;
var connectionString = builder.Configuration.GetConnectionString("Default");
var serverVersion = ServerVersion.AutoDetect(connectionString);

Config.UserClientSecret = builder.Configuration.GetSection("IdentityServerSecrets")["UserClientSecret"];
Config.AdminClientSecret = builder.Configuration.GetSection("IdentityServerSecrets")["AdminClientSecret"];

builder.Services.AddDbContext<IdentityDatabaseContext>(options =>
{
    options.UseMySql(connectionString, serverVersion,
        sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); });
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityDatabaseContext>();
builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;

        options.EmitStaticAudienceClaim = true;
    })
    .AddConfigurationStore(options => options.ConfigureDbContext = b => b.UseMySql(connectionString, serverVersion,
        sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); }))
    .AddOperationalStore(options => options.ConfigureDbContext = b => b.UseMySql(connectionString, serverVersion,
        sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); }))
    .AddAspNetIdentity<ApplicationUser>();

var app = builder.Build();

app.UseIdentityServer();

if (args.Contains("--seed"))
{
    SeedData.EnsureSeedData(app);
    return;
}

app.Run();