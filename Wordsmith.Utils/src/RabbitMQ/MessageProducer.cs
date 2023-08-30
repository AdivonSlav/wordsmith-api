using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Wordsmith.Utils.RabbitMQ;

public class MessageProducer : IMessageProducer
{
    public string SendMessage<T>(string queue, T message)
    {
        var connection = RabbitService.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue, durable: true, exclusive: false);

        var props = channel.CreateBasicProperties();
        props.CorrelationId = Guid.NewGuid().ToString();
        props.ReplyTo = $"{queue}_replies";
        
        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);
        
        channel.BasicPublish("", queue, body: body, basicProperties: props);
        Logger.LogDebug($"Sent message via RabbitMQ on queue {queue}: {jsonString}");

        return props.CorrelationId;
    }
}