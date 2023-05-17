using System.Text;
using EmailConsumer.MessageBroker.Consumers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedLibrary.MessageBroker;

namespace EmailConsumer.MessageBroker;

public class SendEmailMessageBusSubscriber : BackgroundService
{
    private readonly ISendEmailEventConsumer _handler;
    private readonly IModel _channel;
    private readonly string _queueName;

    public SendEmailMessageBusSubscriber(MessageBrokerSettings settings, string queueName,
        ISendEmailEventConsumer handler)
    {
        const string exchange = "example_exchange";
        _queueName = queueName;
        _handler = handler;
        var factory = new ConnectionFactory
        {
            Uri = new Uri($"amqp://{settings.Username}:{settings.Password}@{settings.Host}"),
            DispatchConsumersAsync = true
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queueName, exclusive: false, durable: true, autoDelete: false);
        _channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable:true);
        _channel.QueueBind(queueName, exchange, "send_email_key");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await _handler.HandleEventAsync(message, stoppingToken);
        };
        _channel.BasicConsume(_queueName, true, consumer);
        return Task.CompletedTask;
    }
}