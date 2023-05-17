namespace SharedLibrary.MessageBroker.Contracts;

public record SendEmailEvent(string Recipient, string Subject, string Body, SendEmailType Type)
{
    public override string ToString()
    {
        return $"{Recipient}; {Subject}; {Body}";
    }
}

public enum SendEmailType
{
    SmtpClient
}