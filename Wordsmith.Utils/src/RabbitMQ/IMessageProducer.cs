namespace Wordsmith.Utils.RabbitMQ;

public interface IMessageProducer
{
    /// <summary>
    /// Sends a message to the provided queue 
    /// </summary>
    /// <typeparam name="T">Type of message that is passed</typeparam>
    /// <param name="queue">Name of the queue to send the message to</param>
    /// <param name="message">The message contents</param>
    /// <returns>A correlation ID that can be used for reply purposes</returns>
    public string SendMessage<T>(string queue, T message);
}