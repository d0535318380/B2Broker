using System.Collections.Concurrent;
using Microsoft.IO;

namespace B2Broker.Core;




public class BusMessageWriter : IBusMessageWriter
{
    private readonly BusMessageWriterOption _option;
    private readonly IBusConnection _connection;
    
    private readonly ConcurrentQueue<BusMessageRequest> _queue = new();
    private readonly RecyclableMemoryStreamManager _streamManager = new();
    private int _currentSize;

    public BusMessageWriter(IBusConnection connection, BusMessageWriterOption? option = default)
    {
        _option ??= BusMessageWriterOption.Default;
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public async Task SendMessageAsync(BusMessageRequest request, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var currentSize = Interlocked.Add(ref _currentSize, request.Length);

        _queue.Enqueue(request);
        
        if (currentSize > _option.BufferSize)
        {
            await FlushAsync(token);
        }
    }

    private async Task FlushAsync(CancellationToken token)
    {
        using var stream = _streamManager.GetStream();
        
        Interlocked.Exchange(ref _currentSize, 0);

        while (_queue.TryDequeue(out var item))
        {
            await stream.WriteAsync(item.Message, token);
            
            if (stream.Length > _option.BufferSize)
            {
                break;
            }
        }

        await _connection.PublishAsync(stream.ToArray(), token);
    }
}