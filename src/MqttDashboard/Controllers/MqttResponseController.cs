using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Services;
using MqttDomain.Models;
using MqttHub.Bus;

namespace MqttDashboard.Controllers;

public class MqttResponseController : Controller
{
    private readonly IMqttBus _mqttBus;
    private readonly LogResponseModel _viewModel = new ();

    public MqttResponseController(IMqttBus mqttBus)
    {
        _mqttBus = mqttBus;
        _mqttBus.MessageReceived += OnMessageReceived;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        //await _mqttBus.SubscribeToTopic("Test");

        return PartialView("_MqttResponsePartial", _viewModel);
    }
    
    private async Task OnMessageReceived(string message, string topic)
    {
        _viewModel.Messages.Add($"{topic}: {message}");
        await Task.CompletedTask;
    }
    [HttpPost]
    public async Task<IActionResult> Subscribe(string topic)
    {
        if (string.IsNullOrEmpty(topic)) return RedirectToAction("Index");
        try
        {
            await _mqttBus.SubscribeToTopic(topic);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error while subscribing to {topic}: {e.Message}";
        }

        return RedirectToAction("Index");
    }
    
    // public IActionResult GetReceivedMessages()
    // {
    //     var messages = messageReceiverService.GetReceivedMessages();
    //     return PartialView("_MqttResponsePartial", messages);
    // }
}