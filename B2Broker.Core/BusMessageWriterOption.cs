namespace B2Broker.Core;

public class BusMessageWriterOption
{
    public static readonly BusMessageWriterOption Default = new();
    public int BufferSize { get; set; } = 1000;
}