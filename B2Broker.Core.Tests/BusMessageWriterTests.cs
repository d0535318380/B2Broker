using Moq;
using Moq.AutoMock;

namespace B2Broker.Core.Tests;

public class BusMessageWriterTests
{
    private readonly AutoMocker _mocker = new();
    
    [Fact]
    public async Task PublishMessage_Single_Message()
    {
        // Arrange
        var writer = _mocker.CreateInstance<BusMessageWriter>();
        var message = new byte[1001];
        
        // Act
        await writer.SendMessageAsync(message);
        
        // Assert
        _mocker.Verify<IBusConnection>(x => 
            x.PublishAsync(message, CancellationToken.None), 
            Times.Once);
    }
    
    [Theory]
    [InlineData(10, 101)]
    [InlineData(150, 90)]
    
    public async Task PublishMessage_Multiple_Messages(int count, int size)
    {
        // Arrange
        var options = BusMessageWriterOption.Default;
        var writer = _mocker.CreateInstance<BusMessageWriter>();
        var tasks = new List<Task>();
        
        for (var i = 0; i < count; i++)
        {
            var message = new byte[size];
        
            // Act
           var task = writer.SendMessageAsync(message);
           
           tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        
        // Assert
        _mocker.Verify<IBusConnection>(x => 
            x.PublishAsync(It.Is<byte[]>(p=> p.Length > options.BufferSize), CancellationToken.None), 
            Times.AtLeastOnce);
    }
    
    
    [Fact]
    public async Task PublishMessage_Buffer_Message()
    {
        // Arrange
        var writer = _mocker.CreateInstance<BusMessageWriter>();
        var message = new byte[1];
        
        // Act
        await writer.SendMessageAsync(message);
        
        // Assert
        _mocker.Verify<IBusConnection>(x => 
                x.PublishAsync(message, CancellationToken.None), Times.Never);
    }
}