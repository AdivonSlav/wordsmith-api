using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Wordsmith.IdentityServer.Db.Entities;

namespace Wordsmith.IdentityServer.Startup;

public static class SeedSetup
{
    public static IApplicationBuilder EnsureSeedData(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();

        if (context == null) throw new Exception("ConfigurationDbContext is not registered as a service!");

        EnsureSeedData(context);
        EnsureUsers(scope, configuration);

        return app;
    }

    private static void EnsureSeedData(ConfigurationDbContext context)
    {
        if (!context.Clients.Any())
        {
            foreach (var client in Config.Clients.ToList()) context.Clients.Add(client.ToEntity());
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources.ToList())
                context.IdentityResources.Add(resource.ToEntity());
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes.ToList()) context.ApiScopes.Add(resource.ToEntity());
        }

        if (!context.ApiResources.Any())
        {
            foreach (var resource in Config.ApiResources.ToList()) context.ApiResources.Add(resource.ToEntity());
        }
        
        context.SaveChanges();
    }

    private static void EnsureUsers(IServiceScope scope, IConfiguration configuration)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var adminToCreate = configuration["DefaultAdmin"];
        var userToCreate = configuration["DefaultUser"];

        if (userManager.Users.Any()) return;
        
        if (adminToCreate != null)
        {
                var adminUser = new ApplicationUser()
                {
                    UserName = adminToCreate,
                    Email = $"{adminToCreate}@example.com",
                    EmailConfirmed = true,
                    UserRefId = 1,
                };
                
                var result = userManager.CreateAsync(adminUser, "default$123").Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
                result = userManager.AddClaimsAsync(adminUser, new Claim[]
                {
                    new("role", "admin"),
                    new("user_ref_id", adminUser.UserRefId.ToString()!)
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
        }

        if (userToCreate != null)
        {
                var user = new ApplicationUser()
                {
                    UserName = userToCreate,
                    Email = $"{userToCreate}@example.com",
                    EmailConfirmed = true,
                    UserRefId = 2,
                };
                
                var result = userManager.CreateAsync(user, "default$123").Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
                result = userManager.AddClaimsAsync(user, new Claim[]
                {
                    new("role", "user"),
                    new("user_ref_id", user.UserRefId.ToString()!)
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
        }
    }
}