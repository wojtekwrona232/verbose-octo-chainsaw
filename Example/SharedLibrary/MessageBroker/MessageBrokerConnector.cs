using System.Text;
using RabbitMQ.Client;

namespace SharedLibrary.MessageBroker;

public interface IMessageBrokerConnector
{
    void SendMessage(string message, string routingKey);
}

public class MessageBrokerConnector : IMessageBrokerConnector
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly string _exchangeName;

    public MessageBrokerConnector(MessageBrokerSettings settings)
    {
        _exchangeName = "example_exchange";
        var factory = new ConnectionFactory
        {
            Uri = new Uri($"amqp://{settings.Username}:{settings.Password}@{settings.Host}"),
            DispatchConsumersAsync = true
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void SendMessage(string message, string routingKey)
    {
        _channel.BasicPublish(exchange: _exchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(message));
    }

    public void Dispose()
    {
        if (!_channel.IsOpen) return;
        _channel.Close();
        _connection.Close();
    }
}