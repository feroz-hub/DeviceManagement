using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Services;
using MqttHub.Bus;

namespace MqttDashboard;

public class MqttResponseViewComponent(IMqttBus mqttBus):ViewComponent
{
 
    private IEnumerable<string> _messages { get; set; } = [];

    // public MqttResponseViewComponent(IMqttBus mqttBus)
    // {
    //     _mqttBus = mqttBus;
    //     _mqttBus.MessageReceived += OnMessageReceived;
    //
    // }
    // private  async Task OnMessageReceived(string message, string topic)
    // {
    //     _messages.Add($"Topic: {topic}, Message: {message}");
    //     await Task.CompletedTask; // This is necessary to match the event delegate signature.
    // }
    public async Task<JsonResult> InvokeAsync()
    {

        _messages = mqttBus.GetMessages().ToList();
        // await _mqttBus.SubscribeToTopic("Test");
        // // Returning the view with the messages
        return new JsonResult(_messages);
    }
}