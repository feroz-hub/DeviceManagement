using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Services;
using MqttHub.Bus;

namespace MqttDashboard;

public class MqttResponseViewComponent:ViewComponent
{
    private readonly IMqttBus _mqttBus;
    private List<string> _messages { get; set; } = [];

    public MqttResponseViewComponent(IMqttBus mqttBus)
    {
        _mqttBus = mqttBus;
        _mqttBus.MessageReceived += OnMessageReceived;

    }
    private  async Task OnMessageReceived(string message, string topic)
    {
        _messages.Add($"Topic: {topic}, Message: {message}");
        await Task.CompletedTask; // This is necessary to match the event delegate signature.
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {

        await _mqttBus.SubscribeToTopic("Test");
        // Returning the view with the messages
        return View(_messages);
    }
}