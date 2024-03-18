using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Wordsmith.IdentityServer.Db.Entities;
using Wordsmith.Utils;

namespace Wordsmith.IdentityServer.Db;

public static class DatabaseSeeds
{
    public static void EnsureSeedData(ConfigurationDbContext configContext, UserManager<ApplicationUser> userManager)
    {
        Logger.LogInfo("Checking whether seeding is necessary...");

        CreateIdentity(configContext);
        CreateUsers(userManager);
        
        configContext.SaveChanges();
    }

    private static void CreateIdentity(ConfigurationDbContext context)
    {
        if (!context.Clients.Any())
        {
            foreach (var client in Config.Clients.ToList()) context.Clients.Add(client.ToEntity());
            Logger.LogInfo("Seeded Identity clients");
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources.ToList())
                context.IdentityResources.Add(resource.ToEntity());
            Logger.LogInfo("Seeded Identity resources");
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes.ToList()) context.ApiScopes.Add(resource.ToEntity());
            Logger.LogInfo("Seeded Identity API scopes");
        }

        if (!context.ApiResources.Any())
        {
            foreach (var resource in Config.ApiResources.ToList()) context.ApiResources.Add(resource.ToEntity());
            Logger.LogInfo("Seeded Identity API resources");
        }
    }

    private static void CreateUsers(UserManager<ApplicationUser> userManager)
    {
        if (userManager.Users.Any())
        {
            return;
        }
        
        const string defaultPassword = "default$123";
        
        var adminUser = new ApplicationUser()
        {
            UserName = "orwell47",
            Email = "orwell47@personal.com",
            EmailConfirmed = true,
            UserRefId = 1,
        };
        
        var adminAddResult = userManager.CreateAsync(adminUser, defaultPassword).Result;
        if (!adminAddResult.Succeeded) throw new Exception(adminAddResult.Errors.First().Description);
        
        var adminClaimsResult = userManager.AddClaimsAsync(adminUser, new Claim[]
        {
            new("role", "admin"),
            new("user_ref_id", adminUser.UserRefId.ToString()!)
        }).Result;
        if (!adminClaimsResult.Succeeded) throw new Exception(adminClaimsResult.Errors.First().Description);
        
        var user = new ApplicationUser()
        {
            UserName = "john_doe1",
            Email = $"john_doe1@personal.com",
            EmailConfirmed = true,
            UserRefId = 2,
        };
        
        var userAddResult = userManager.CreateAsync(user, defaultPassword).Result;
        if (!userAddResult.Succeeded) throw new Exception(userAddResult.Errors.First().Description);
        
        var userClaimsResult = userManager.AddClaimsAsync(user, new Claim[]
        {
            new("role", "user"),
            new("user_ref_id", user.UserRefId.ToString()!)
        }).Result;
        if (!userClaimsResult.Succeeded) throw new Exception(userClaimsResult.Errors.First().Description);

        var seller = new ApplicationUser()
        {
            UserName = "jane_doe2",
            Email = "jane_doe2@personal.com",
            EmailConfirmed = true,
            UserRefId = 3,
        };

        var sellerAddResult = userManager.CreateAsync(seller, defaultPassword).Result;
        if (!sellerAddResult.Succeeded) throw new Exception(sellerAddResult.Errors.First().Description);
        
        var sellerClaimsResult = userManager.AddClaimsAsync(seller, new Claim[]
        {
            new("role", "user"),
            new("user_ref_id", seller.UserRefId.ToString()!)
        }).Result;
        if (!sellerClaimsResult.Succeeded) throw new Exception(sellerClaimsResult.Errors.First().Description);
        
        Logger.LogInfo("Seeded users");
    }
}