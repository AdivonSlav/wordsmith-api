using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wordsmith.IdentityServer.Db.Entities;

namespace Wordsmith.IdentityServer.Db;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();
            EnsureSeedData(context);
            EnsureUsers(scope);
        }
    }

    private static void EnsureSeedData(ConfigurationDbContext context)
    {
        if (!context.Clients.Any())
        {
            foreach (var client in Config.Clients.ToList()) context.Clients.Add(client.ToEntity());

            context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources.ToList())
                context.IdentityResources.Add(resource.ToEntity());

            context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes.ToList()) context.ApiScopes.Add(resource.ToEntity());

            context.SaveChanges();
        }

        if (!context.ApiResources.Any())
        {
            foreach (var resource in Config.ApiResources.ToList()) context.ApiResources.Add(resource.ToEntity());

            context.SaveChanges();
        }

        if (!context.IdentityProviders.Any())
        {
            context.IdentityProviders.Add(new OidcProvider
            {
                Scheme = "demoidsrv",
                DisplayName = "IdentityServer",
                Authority = "https://demo.duendesoftware.com",
                ClientId = "login"
            }.ToEntity());
            context.SaveChanges();
        }
    }

    private static void EnsureUsers(IServiceScope scope)
    {
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var jane = userMgr.FindByNameAsync("jane").Result;
        if (jane == null)
        {
            jane = new ApplicationUser()
            {
                UserName = "jane",
                Email = "JaneDoe@email.com",
                EmailConfirmed = true,
                UserRefId = 1
            };
            var result = userMgr.CreateAsync(jane, "Pass123$").Result;
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

            result = userMgr.AddClaimsAsync(jane, new Claim[]
            {
                new(JwtClaimTypes.Name, "Jane Doe"),
                new(JwtClaimTypes.GivenName, "Jane"),
                new(JwtClaimTypes.FamilyName, "Doe"),
                new(JwtClaimTypes.WebSite, "http://jane.com"),
                new("role", "user"),
                new("user_ref_id", jane.UserRefId.ToString()!)
            }).Result;
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
        }

        var bob = userMgr.FindByNameAsync("bob").Result;
        if (bob == null)
        {
            bob = new ApplicationUser()
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(bob, "Pass123$").Result;
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

            result = userMgr.AddClaimsAsync(bob, new Claim[]
            {
                new(JwtClaimTypes.Name, "Bob Smith"),
                new(JwtClaimTypes.GivenName, "Bob"),
                new(JwtClaimTypes.FamilyName, "Smith"),
                new(JwtClaimTypes.WebSite, "http://bob.com"),
                new("role", "user"),
                new("user_ref_id", "1")
            }).Result;
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
        }
    }
}