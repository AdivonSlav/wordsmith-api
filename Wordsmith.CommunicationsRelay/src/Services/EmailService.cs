using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Utils;

namespace Wordsmith.CommunicationsRelay.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<EmailSettings> _emailSettings;
    
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings;
    }
    
    public async Task<bool> SendEmail(SendEmailMessage message)
    {
        var email = new MimeMessage();

        try
        {
            email.From.Add(MailboxAddress.Parse(_emailSettings.Value.SenderEmail));
            email.To.Add(MailboxAddress.Parse(message.EmailToId));
            email.Subject = message.EmailSubject;
            email.Body = new TextPart(TextFormat.Html) { Text = message.EmailBody };
        }
        catch (Exception e)
        {
            Logger.LogError("Could not prepare email message!", e);
            return false;
        }
        
        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_emailSettings.Value.Server, _emailSettings.Value.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Value.Username, _emailSettings.Value.Password);
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            Logger.LogError("Could not send email!", e);
            return false;
        }

        return true;
    }
}