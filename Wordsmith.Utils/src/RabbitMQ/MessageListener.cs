using System.Text;
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
    
    /// <summary>
    /// Registers the provided callback to be executed when a message is received via the provided queue
    /// </summary>
    public Task Listen(string queue, MessageReceivedCallback callback)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (channel, ea) =>
        {
            var body = ea.Body.ToArray();
            var text = Encoding.UTF8.GetString(body);

            await callback(text);
            
            Logger.LogDebug($"Received message {text}");

            _channel.BasicAck(ea.DeliveryTag, false);
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