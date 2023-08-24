namespace Wordsmith.Utils.RabbitMQ;

public delegate Task MessageReceivedCallback(string message);

public interface IMessageListener
{
    public Task Listen(string queue, MessageReceivedCallback callback);
}