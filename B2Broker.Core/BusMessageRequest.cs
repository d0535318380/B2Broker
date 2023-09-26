namespace B2Broker.Core;

public class BusMessageRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Length => Message.Length;
    public byte[] Message { get; set; } = Array.Empty<byte>();

    public BusMessageRequest()
    {
    }

    public BusMessageRequest(byte[] message)
    {
        Message = message;
    }
}