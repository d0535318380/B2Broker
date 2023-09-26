namespace B2Broker.Core;

public class BusMessageRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Length => Message.Length;
    public byte[] Message { get; set; }

    public BusMessageRequest(byte[]? message = default)
    {
        Message = message ?? Array.Empty<byte>();
    }
}