using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Services;
using MqttHub.Bus;

namespace MqttDashboard.Controllers;

public class MqttResponseController(IMqttBus mqttBus,IMessageReceiverService messageReceiverService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Subscribe(string topic)
    {
        if (string.IsNullOrEmpty(topic)) return RedirectToAction("Index");
        try
        {
            await mqttBus.SubscribeToTopic(topic);
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = $"Error while subscribing to {topic}: {e.Message}";
        }

        return RedirectToAction("Index");
    }
    
    public IActionResult GetReceivedMessages()
    {
        var messages = messageReceiverService.GetReceivedMessages();
        return PartialView("_MqttResponsePartial", messages);
    }
}