using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Services;
using MqttDomain.Models;
using MqttHub.Bus;

namespace MqttDashboard;

public class MqttResponseViewComponent:ViewComponent
{
    private readonly IMqttBus _mqttBus;
    private readonly LogResponseModel _viewModel = new ();

    public MqttResponseViewComponent(IMqttBus mqttBus)
    {
        _mqttBus = mqttBus;
        _mqttBus.MessageReceived += OnMessageReceived;
    
    }
    private  async Task OnMessageReceived(string message, string topic)
    {
        _viewModel.Messages.Add($"Topic: {topic}, Message: {message}");
        await Task.CompletedTask; // This is necessary to match the event delegate signature.
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
       //_viewModel.Messages.Add("Feroz");
        return View(_viewModel);
    }
}