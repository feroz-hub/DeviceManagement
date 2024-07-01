using System.Collections.Concurrent;
using MqttHub.Bus;

namespace MqttDashboard.Services;

public class MessageReceiverService(IMqttBus mqttBus):IMessageReceiverService
{
    private readonly ConcurrentBag<string> _receivedMessages = [];
    private Action<string, string>? _messageHandler;

    public async Task StartAsync(Action<string, string> messageHandler)
    {
        _messageHandler = messageHandler;
        mqttBus.MessageReceived += MqttBus_MessageReceived;
        await Task.CompletedTask;
    }
    public async Task StopAsync()
    {
        mqttBus.MessageReceived -= MqttBus_MessageReceived;
        _messageHandler = null;
        await Task.CompletedTask; 
    }

    public IEnumerable<string> GetReceivedMessages()
    {
        return _receivedMessages.ToList();
    }
    private async Task MqttBus_MessageReceived(string message, string topic)
    {
        _receivedMessages.Add($"Topic: {topic}, Message: {message}");
        if (_messageHandler != null)
        {
            try
            {
                _messageHandler.Invoke(message, topic);
            }
            catch (Exception ex)
            {
                // Handle any exceptions from message handling
                Console.WriteLine($"Error handling MQTT message: {ex.Message}");
            }
        }
    }
}