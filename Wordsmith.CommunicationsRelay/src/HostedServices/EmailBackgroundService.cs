using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Wordsmith.CommunicationsRelay.Services;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.CommunicationsRelay.HostedServices;

public class EmailBackgroundService : BackgroundService
{
    private readonly IMessageListener _messageListener;
    private readonly IEmailService _emailService;

    public EmailBackgroundService(IMessageListener messageListener, IEmailService emailService)
    {
        _messageListener = messageListener;
        _emailService = emailService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInfo("Starting background service for email operations...");
        await _messageListener.Listen("send_email", HandleSendingEmail);
    }

    private async Task<OperationStatusMessage> HandleSendingEmail(string message, string? correlationId)
    {
        SendEmailMessage? emailMessage;
        var status = new OperationStatusMessage()
        {
            Succeeded = false
        };

        try
        {
            emailMessage = JsonSerializer.Deserialize<SendEmailMessage>(message);
        }
        catch (Exception e)
        {
            Logger.LogError("Could not deserialize RabbitMQ message as an email message", e);
            status.Errors.Add(e.Message);
            return status;
        }

        if (emailMessage == null)
        {
            Logger.LogError("Could not deserialize RabbitMQ message as an email message");
            return status;
        }
        
        if (!await _emailService.SendEmail(emailMessage))
        {
            status.Errors.Add("Could not send email");
            return status;
        }
        
        Logger.LogInfo($"Sent email to {emailMessage.EmailToId}");

        status.Succeeded = true;
        status.Errors.Clear();
        return status;
    }
}