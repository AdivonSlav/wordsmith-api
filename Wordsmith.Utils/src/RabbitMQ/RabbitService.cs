using RabbitMQ.Client;

namespace Wordsmith.Utils.RabbitMQ;

public static class RabbitService
{
    private static ConnectionFactory _connectionFactory = null!;

    /// <summary>
    /// Initializes the ConnectionFactory of the RabbitService
    /// </summary>
    /// <param name="hostname">Hostname of the RabbitMQ server</param>
    /// <param name="username">Username of the RabbitMQ server</param>
    /// <param name="password">Password of the RabbitMQ server</param>
    /// <exception cref="Exception">Invalid settings passed</exception>
    public static void Init(string? hostname, string? username, string? password)
    {
        if (hostname == null || username == null || password == null)
        {
            throw new Exception("Not all RabbitMQ connection details have been set!");
        }
        
        _connectionFactory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password,
            VirtualHost = "/",
            Port = AmqpTcpEndpoint.UseDefaultPort,
            DispatchConsumersAsync = true,
        };

        Logger.LogDebug("Initialized RabbitMQ connection factory");
    }

    /// <summary>
    /// Creates a connection using the connection details passed to the factory with Init()
    /// </summary>
    public static IConnection CreateConnection()
    {
        return _connectionFactory.CreateConnection();
    }
}