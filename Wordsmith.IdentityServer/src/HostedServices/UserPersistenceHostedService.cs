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
        await _messageListener.ListenAndReply("user_insertion", HandleInsert);
        await _messageListener.ListenAndReply("user_update", HandleUpdate);
    }

    private async Task<OperationStatusMessage> HandleInsert(string message, string? correlationId)
    {
        using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        var asUser = JsonSerializer.Deserialize<RegisterUserMessage>(message);

        if (asUser == null)
        {
            throw new Exception("Unable to deserialize JSON string to RegisterUserMessage");
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
            return new OperationStatusMessage()
            {
                Succeeded = false, Errors = GetIdentityErrors(creationResult.Errors)
            };
        }

        creationResult = await userManager.AddClaimsAsync(newUser, new Claim[]
        {
            new("role", "user"),
            new("user_ref_id", newUser.UserRefId.ToString()!)
        });
            
        if (!creationResult.Succeeded)
        {
            return new OperationStatusMessage()
            {
                Succeeded = false, Errors = GetIdentityErrors(creationResult.Errors)
            };
        }

        return new OperationStatusMessage()
        {
            Succeeded = true
        };
    }

    private async Task<OperationStatusMessage> HandleUpdate(string message, string? correlationId)
    {
        using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var asUpdateMessage = JsonSerializer.Deserialize<UpdateUserMessage>(message);

        if (asUpdateMessage == null)
        {
            return new OperationStatusMessage()
                { Succeeded = false, Errors = { "Could not deserialize message into update message object! " } };
        }

        var temp = await userManager.GetUsersForClaimAsync(new Claim("user_ref_id", asUpdateMessage.Id.ToString()));

        if (temp.Count == 0)
        {
            return new OperationStatusMessage()
                { Succeeded = false, Errors = { "User was not found in the IdentityServer datastore!" } };
        }
        
        var user = temp[0];

        if (!string.IsNullOrEmpty(asUpdateMessage.Username))
        {
            var usernameResult = await userManager.SetUserNameAsync(user, asUpdateMessage.Username);
            
            if (!usernameResult.Succeeded)
            {
                return new OperationStatusMessage()
                    { Succeeded = false, Errors = GetIdentityErrors(usernameResult.Errors) };  
            }
        }

        if (!string.IsNullOrEmpty(asUpdateMessage.Email))
        {
            var emailResult = await userManager.SetEmailAsync(user, asUpdateMessage.Email);
            if (!emailResult.Succeeded)
            {
                return new OperationStatusMessage()
                    { Succeeded = false, Errors = GetIdentityErrors(emailResult.Errors) };  
            }
        }
        
        if (!string.IsNullOrEmpty(asUpdateMessage.Password))
        {
            var passwordResult = await userManager.ChangePasswordAsync(user, asUpdateMessage.OldPassword, asUpdateMessage.Password);
            if (!passwordResult.Succeeded)
            {
                return new OperationStatusMessage()
                    { Succeeded = false, Errors = GetIdentityErrors(passwordResult.Errors) };  
            }
        }

        return new OperationStatusMessage()
        {
            Succeeded = true
        };
    }

    private static List<string> GetIdentityErrors(IEnumerable<IdentityError> errors)
    {
        return errors.Select(error => error.Description).ToList();
    }
}