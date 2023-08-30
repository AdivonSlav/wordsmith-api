namespace Wordsmith.Utils.RabbitMQ;

public interface IMessageProducer
{
    /// <summary>
    /// Sends a message to the provided queue and returns the correlation ID for reply purposes 
    /// </summary>
    public string SendMessage<T>(string queue, T message);
}