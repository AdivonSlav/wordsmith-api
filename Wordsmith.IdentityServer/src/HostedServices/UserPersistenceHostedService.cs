using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Wordsmith.IdentityServer.Db.Entities;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.IdentityServer.HostedServices;

public class UserPersistenceHostedService : BackgroundService
{
    private readonly IMessageListener _messageListener;
    private readonly IServiceProvider _serviceProvider;

    public UserPersistenceHostedService(IMessageListener messageListener, IServiceProvider serviceProvider)
    {
        _messageListener = messageListener;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageListener.Listen("user_insertion", HandleInsert);
    }

    private async Task HandleInsert(string message)
    {
        using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        var asUser = JsonSerializer.Deserialize<RegisterUserMessage>(message);

        if (asUser == null)
        {
            throw new Exception("Unable to deserialize JSON string to UserMessage");
        }
            
        var newUser = new ApplicationUser()
        {
            UserName = asUser.Username,
            Email = asUser.Email,
            EmailConfirmed = true,
            UserRefId = asUser.Id,
        };

        var creationResult = await userManager.CreateAsync(newUser, asUser.Password);

        if (!creationResult.Succeeded)
        {
            throw new Exception($"Unable to persist user {asUser.Username} to the backing store");
        }

        creationResult = await userManager.AddClaimsAsync(newUser, new Claim[]
        {
            new("role", "user"),
            new("user_ref_id", newUser.UserRefId.ToString()!)
        });
            
        if (!creationResult.Succeeded)
        {
            throw new Exception($"Unable to persist claims for user {asUser.Username}");
        }
    }
}