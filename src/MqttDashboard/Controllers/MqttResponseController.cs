using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Services;
using MqttHub.Bus;

namespace MqttDashboard.Controllers;

public class MqttResponseController : Controller
{
    private readonly IMqttBus _mqttBus;
    private static List<string> _messages = [];

    public MqttResponseController(IMqttBus mqttBus)
    {
        _mqttBus = mqttBus;
        _mqttBus.MessageReceived += OnMessageReceived;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        //await _mqttBus.SubscribeToTopic("Test");

        return View(_messages);
    }
    
    private async Task OnMessageReceived(string message, string topic)
    {
        _messages.Add($"{topic}: {message}");
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