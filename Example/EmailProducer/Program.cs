using EmailProducer.Services;
using Microsoft.Extensions.Options;
using SharedLibrary.MessageBroker;
using SharedLibrary.MessageBroker.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
builder.Services.AddSingleton<IMessageBrokerConnector, MessageBrokerConnector>();
builder.Services.AddScoped<ISendEmailProducer, SendEmailProducer>();
var app = builder.Build();

app.MapGet("/", (ISendEmailProducer producer) =>
{
    producer.HandleProduce(new SendEmailEvent("example@example.com", "This is a test subject",
        "This is a test body", SendEmailType.SmtpClient));
    return "Hello world";
});

app.Run();