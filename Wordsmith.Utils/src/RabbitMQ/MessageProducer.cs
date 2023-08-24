using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Wordsmith.Utils.RabbitMQ;

public class MessageProducer : IMessageProducer
{
    /// <summary>
    /// Sends a message to the provided queue
    /// </summary>
    public void SendMessage<T>(string queue, T message)
    {
        var connection = RabbitService.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue, durable: true, exclusive: false);

        var jsonString = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonString);
        
        channel.BasicPublish("", queue, body: body);
        Logger.LogDebug($"Sent message via RabbitMQ {jsonString}");
    }
}