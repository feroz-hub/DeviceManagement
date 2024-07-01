using System.Collections.Concurrent;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace MqttHub.Bus;

public class MqttBus(ISender mediator,IMqttClient mqttClient,IServiceScopeFactory scopeFactory,IManagedMqttClient managedMqttClient):IMqttBus
{
    private readonly ConcurrentBag<string> _messages;
    public readonly ConcurrentBag<string>  _subscribedTopics;

    private readonly MqttFactory _mqttFactory=new() ;
    public Task SendCommand<T>(T command) where T : Commands.Command
    {
        return mediator.Send(command);
    }
    public async Task ManagedMqttPublish<T>(T message, string topic) where T : class
    {
        var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

        if (await ConnectMqttClientAsync() || mqttClient!= null && mqttClient.IsConnected )
        {

            await mqttClient.PublishAsync(new MqttApplicationMessage()
            {
                Topic = topic,
                PayloadSegment = data,
                QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                Retain = true
            });
        }
    }

    public async Task SubscribeToTopic(string topic)
    {
        if (await ConnectMqttClientAsync() || mqttClient!=null && mqttClient.IsConnected)
        {
            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var message=e.ApplicationMessage.ConvertPayloadToString();
                if (MessageReceived != null)
                    await MessageReceived.Invoke(message, e.ApplicationMessage.Topic);
                await Task.CompletedTask;
            };
            await mqttClient.SubscribeAsync(topic);
            
        }
    }
    public IEnumerable<string> GetMessages()
    {
        return _messages;
    }
    public IEnumerable<string> GetSubscribedTopics()
    {
        return _subscribedTopics.Distinct();
    }

    public event Func<string, string, Task>? MessageReceived;

    private async Task<bool> ConnectManagedMqttClientAsync()
    {
        var result = false;
        try
        {
            if (!managedMqttClient.IsConnected)
            {
                var managedClientOption = new MqttClientOptionsBuilder()
                    .WithClientId("AdminManagedMqttClient")
                    .WithTcpServer("localhost", 1883)
                    .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .WithCleanSession(true)
                    .WithKeepAlivePeriod(TimeSpan.FromHours(2));
                managedMqttClient = _mqttFactory.CreateManagedMqttClient();

                var managedMqttClientOption = new ManagedMqttClientOptionsBuilder()
                    .WithClientOptions(managedClientOption)
                    .WithMaxPendingMessages(10)
                    //.WithPendingMessagesOverflowStrategy(MqttPendingMessagesOverflowStrategy.DropOldestQueuedMessage)
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(1))
                    .Build();
                await managedMqttClient.StartAsync(managedMqttClientOption);
                result = true;
            }
        }
        catch (Exception ex)
        {
            result = false;
        }
        return result;
    }
    private async Task<bool> ConnectMqttClientAsync()
    {
        var  result=false;
        try
        {
            if (!mqttClient.IsConnected)
            {
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer("localhost", 1883)
                    .WithClientId("AdminSubscribeClient")
                    .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .WithKeepAlivePeriod(TimeSpan.FromMinutes(60))
                    .WithCleanSession()
                    .Build();
                mqttClient = _mqttFactory.CreateMqttClient();
                await mqttClient.ConnectAsync(options);
                result = true;
            }
        }
        catch (Exception e)
        {
            result=false;
        }
        return result;
    }
   
    private async Task SubscribeToTopics(List<string> topics)
    {
        foreach (var topic in topics)
        {
             await managedMqttClient.SubscribeAsync(topic);
            _subscribedTopics.Add(topic);
        }
    }

}