namespace B2Broker.Core;

public static class BusMessageWriterExtensions
{
    public static Task SendMessageAsync(this IBusMessageWriter context, byte[] message, CancellationToken token = default) 
        => context.SendMessageAsync(new BusMessageRequest(message), token);
}