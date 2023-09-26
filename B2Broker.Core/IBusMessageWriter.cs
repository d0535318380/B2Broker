namespace B2Broker.Core;

public interface IBusMessageWriter
{
    Task SendMessageAsync(BusMessageRequest request, CancellationToken token = default);
}