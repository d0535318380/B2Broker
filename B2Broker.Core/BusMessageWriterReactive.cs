using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.IO;

namespace B2Broker.Core;

public class BusMessageWriterReactive : IBusMessageWriter
{
    private readonly IBusConnection _connection;
    private readonly RecyclableMemoryStreamManager _streamManager = new();
    
    private readonly Subject<BusMessageRequest> _messageSubject = new();
    private readonly IObservable<BusMessageRequest[]> _messageObservable;
    private readonly IDisposable _subscription;

    public BusMessageWriterReactive(IBusConnection connection, BusMessageWriterOption? option = default)
    {
        option ??=  BusMessageWriterOption.Default;
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));

        _messageObservable = _messageSubject.AsObservable()
            .Buffer(TimeSpan.FromMilliseconds(100))
            .Where(messages => messages.Sum(x=>x.Length) > option.BufferSize)
            .Select(messages => messages.ToArray());

        _subscription = _messageObservable
            .Subscribe(async messages => { await FlushAsync(messages); });
    }

    private async Task FlushAsync(IEnumerable<BusMessageRequest> messages)
    {
        using var stream = _streamManager.GetStream();

        foreach (var message in messages)
        {
            await stream.WriteAsync(message.Message);
        }

        await _connection.PublishAsync(stream.ToArray());
    }

    public Task SendMessageAsync(BusMessageRequest request, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        _messageSubject.OnNext(request);
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _subscription.Dispose();
        _messageSubject.Dispose();
    }
}