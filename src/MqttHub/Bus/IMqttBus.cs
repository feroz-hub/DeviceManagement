namespace MqttHub.Bus;

public interface IMqttBus
{
    Task ManagedMqttPublish<T>(T message,string topic) where T : class;
    Task SubscribeToTopic(string topic);
    IEnumerable<string> GetMessages();
    IEnumerable<string> GetSubscribedTopics();
}