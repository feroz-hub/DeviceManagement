
namespace MqttHub.Bus;


public interface IMqttBus
{
    Task ManagedMqttPublish<T>(T message,string topic) where T : class;
     Task SendCommand<T>(T command) where T : Commands.Command;
    Task SubscribeToTopic(string topic);
    IEnumerable<string> GetMessages();
    IEnumerable<string> GetSubscribedTopics();
    event Func<string, string, Task> MessageReceived;
}