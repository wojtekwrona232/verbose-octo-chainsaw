using System.Text.Json;
using SharedLibrary.MessageBroker;
using SharedLibrary.MessageBroker.Contracts;

namespace EmailProducer.Services;

public interface ISendEmailProducer : IProducer<SendEmailEvent>
{
}

public class SendEmailProducer : ISendEmailProducer
{
    private readonly IMessageBrokerConnector _connector;
    private const string RoutingKey = "send_email_key";

    public SendEmailProducer(IMessageBrokerConnector connector)
    {
        _connector = connector;
    }

    public void HandleProduce(SendEmailEvent message)
    {
        var serializedMessage = JsonSerializer.Serialize(message);
        _connector.SendMessage(serializedMessage, RoutingKey);
    }
}