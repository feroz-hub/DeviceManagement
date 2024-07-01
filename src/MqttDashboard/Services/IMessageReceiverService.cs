namespace MqttDashboard.Services;

public interface IMessageReceiverService
{
    Task StartAsync(Action<string, string> messageHandler);
    Task StopAsync();
    IEnumerable<string> GetReceivedMessages();
}