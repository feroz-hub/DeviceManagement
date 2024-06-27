using Mapster;
using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Models;
using MqttDomain.Models;
using MqttHub.Bus;
using MqttHub.Services;

namespace MqttDashboard.Controllers;

public class LogRequestController (IMqttService mqttService,IMqttBus mqttBus): Controller
{
    
    // GET
    public IActionResult Index()
    {
        var messages = mqttBus.GetMessages();
        var logResponseModel = new LogResponseModel
        {
            Messages = messages != null ? messages.ToList() : []
        };
        var model = new LogRequestAndResponseModel()
        {
            LogRequestModel = new LogRequestDto(),
            LogResponseModel = logResponseModel
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LogRequestDto logRequestModel)
    {
        if (ModelState.IsValid)
        {
            var dto = new LogRequestModel
            {
                RequestId = Guid.NewGuid(),
                SourceId = "AdminClient",
                LogRequestDto = logRequestModel,
                RequestDate = DateTime.Now
            };
            await Subscribe("Test");
            mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
            // Perform your logic here

           
            return RedirectToAction("Index"); // Or wherever you want to redirect
        }
        
        var messages = mqttBus.GetMessages();
        var logResponseModel = new LogResponseModel()
        {
            Messages = messages != null ? messages.ToList() : []

        };
        var model = new LogRequestAndResponseModel()
        {
            LogRequestModel = new LogRequestDto(),
            LogResponseModel = logResponseModel
        };
        // Process the data
        // Convert the ViewModel to the DTO and save or use it as required
        return View("Index",model);
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

    [HttpGet]
    public IActionResult GetMessages()
    {
        var messages = mqttBus.GetMessages();
        return Json(messages);
    }
    public IActionResult Privacy()
    {
        return View();
    }
}