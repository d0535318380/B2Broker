namespace B2Broker.Core;

public interface IBusConnection
{
    Task PublishAsync(byte[] toArray, CancellationToken token = default);
}