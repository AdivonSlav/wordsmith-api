namespace Wordsmith.Utils.RabbitMQ;

public interface IMessageProducer
{
    public void SendMessage<T>(string queue, T message);
}