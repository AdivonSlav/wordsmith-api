using Wordsmith.Models.MessageObjects;

namespace Wordsmith.CommunicationsRelay.Services;

public interface IEmailService
{
    Task<bool> SendEmail(SendEmailMessage message);
}