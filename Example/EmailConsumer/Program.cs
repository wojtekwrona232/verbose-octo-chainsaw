using EmailConsumer;
using EmailConsumer.MessageBroker;
using EmailConsumer.MessageBroker.Consumers;
using Microsoft.Extensions.Options;
using SharedLibrary.MessageBroker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.Configure<MailKitSettings>(builder.Configuration.GetSection("MailKit"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MailKitSettings>>().Value);

builder.Services.AddSingleton<ISendEmailEventConsumer, SendEmailEventConsumer>();

builder.Services.AddHostedService(x => new SendEmailMessageBusSubscriber(
    settings: x.GetRequiredService<MessageBrokerSettings>(),
    queueName: "send_email",
    handler: x.GetRequiredService<ISendEmailEventConsumer>()));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();