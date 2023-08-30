using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Wordsmith.Utils.RabbitMQ;

public class MessageListener : IMessageListener, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageListener()
    {
        _connection = RabbitService.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public Task ListenAndReply(string queue, MessageReceivedCallback callback)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (channel, ea) =>
        {
            var body = ea.Body.ToArray();
            var text = Encoding.UTF8.GetString(body);

            var result = await callback(text, null);
            
            _channel.BasicAck(ea.DeliveryTag, false);
            Logger.LogDebug($"Received message via RabbitMQ on queue {queue}: {text}");

            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

            var replyJson = JsonSerializer.Serialize(result);
            var replyBody = Encoding.UTF8.GetBytes(replyJson);
            
            _channel.BasicPublish("", ea.BasicProperties.ReplyTo, replyProps, replyBody);
            Logger.LogDebug($"Sent message via RabbitMQ on queue {ea.BasicProperties.ReplyTo}: {replyJson}");
        };

        _channel.BasicConsume(queue, false, consumer);
        return Task.CompletedTask;
    }

    public Task Listen(string queue, MessageReceivedCallback callback)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.Received += async (channel, ea) =>
        {
            var body = ea.Body.ToArray();
            var text = Encoding.UTF8.GetString(body);

            await callback(text, ea.BasicProperties.CorrelationId);
            _channel.BasicAck(ea.DeliveryTag, false);
            Logger.LogDebug($"Received message via RabbitMQ on queue {queue}: {text}");
        };

        _channel.BasicConsume(queue, false, consumer);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_channel.IsOpen) _channel.Close();
        if (_connection.IsOpen) _connection.Close();
        
        GC.SuppressFinalize(this);
    }
}