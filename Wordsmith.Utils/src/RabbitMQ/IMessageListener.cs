using Wordsmith.Models.MessageObjects;

namespace Wordsmith.Utils.RabbitMQ;

/// <summary>
/// The callback that can be ran after a message is received on a RabbitMQ queue.
/// </summary>
public delegate Task<OperationStatusMessage> MessageReceivedCallback(string message, string? correlationId);

public interface IMessageListener
{
    /// <summary>
    /// Registers the provided callback to be executed when a message is received via the provided queue.
    /// Additionally, it replies with the return value of the callback
    /// </summary>
    /// <param name="queue">The queue that should be listened for</param>
    /// <param name="callback">The callback that should be executed on message arrival</param>
    public Task ListenAndReply(string queue, MessageReceivedCallback callback);

    /// <summary>
    /// Registers the provided callback to be executed when a message is received via the provided queue.
    /// </summary>
    /// <param name="queue">The queue that should be listened for</param>
    /// <param name="callback">The callback that should be executed on message arrival</param>
    public Task Listen(string queue, MessageReceivedCallback callback);
}