using System.Collections.Concurrent;

namespace B2Broker.Core;

public class BusMessageWriter : IBusMessageWriter
{
    private readonly BusMessageWriterOption _option;
    private readonly IBusConnection _connection;
    private readonly ConcurrentQueue<BusMessageRequest> _queue = new();
    
    private int _currentSize = 0;


    public BusMessageWriter(IBusConnection connection) :
        this(BusMessageWriterOption.Default, connection)
    {
        _connection = connection;
    }

    public BusMessageWriter(BusMessageWriterOption option, IBusConnection connection)
    {
        _option = option;
        _connection = connection;
    }

    public async Task SendMessageAsync(BusMessageRequest request, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var currentSize = Interlocked.Add(ref _currentSize, request.Length);

        _queue.Enqueue(request);

        if (currentSize <= _option.BufferSize)
        {
            return;
        }

        await FlushAsync(token);
    }

    private async Task FlushAsync(CancellationToken token)
    {
        var size = 0;
        using var stream = new MemoryStream();

        while (_queue.TryDequeue(out var item)
               && size <= _option.BufferSize)
        {
            await stream.WriteAsync(item.Message, token);
            size += item.Length;
        }

        Interlocked.Exchange(ref _currentSize, 0);
        await _connection.PublishAsync(stream.ToArray(), token);
    }
}