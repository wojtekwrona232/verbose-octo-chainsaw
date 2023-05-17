namespace SharedLibrary.MessageBroker;

public interface IProducer<in T>
{
    void HandleProduce(T message);
}