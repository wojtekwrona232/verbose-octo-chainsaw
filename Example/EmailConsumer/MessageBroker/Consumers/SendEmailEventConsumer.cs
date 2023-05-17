using System.Text.Json;
using MailKit.Net.Smtp;
using MimeKit;
using SharedLibrary.MessageBroker.Contracts;

namespace EmailConsumer.MessageBroker.Consumers;

public interface ISendEmailEventConsumer
{
    Task HandleEventAsync(string message, CancellationToken cancellationToken);
}

public class SendEmailEventConsumer : ISendEmailEventConsumer
{
    private readonly MailKitSettings _settings;

    public SendEmailEventConsumer(MailKitSettings settings)
    {
        _settings = settings;
    }

    public async Task HandleEventAsync(string message, CancellationToken cancellationToken)
    {
        try
        {
            var msg = JsonSerializer.Deserialize<SendEmailEvent>(message);
            if (msg is null) return;

            switch (msg.Type)
            {
                case SendEmailType.SmtpClient:
                    Console.WriteLine(msg);
                    
                    var email = new MimeMessage();
                    email.To.Add(MailboxAddress.Parse(msg.Recipient));
                    email.Subject = msg.Subject;
                    email.Body = new TextPart(msg.Body);

                    try
                    {
                        using var smtp = new SmtpClient();
                        await smtp.ConnectAsync(_settings.Host, _settings.Port, cancellationToken: cancellationToken);
                        await smtp.AuthenticateAsync(_settings.Login, _settings.Password,
                            cancellationToken: cancellationToken);
                        await smtp.SendAsync(email, cancellationToken: cancellationToken);
                        await smtp.DisconnectAsync(true, cancellationToken: cancellationToken);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    break;
                default:
                    return;
            }
        }
        catch (Exception)
        {
            return;
        }
    }
}