using Mapster;
using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Models;
using MqttDomain.Models;
using MqttHub.Bus;
using MqttHub.Services;

namespace MqttDashboard.Controllers;

public class LogRequestController : Controller
{
    private readonly IMqttService _mqttService;
    private readonly IMqttBus _mqttBus;
    private readonly LogResponseModel _viewModel = new ();

    public LogRequestController(IMqttService mqttService, IMqttBus mqttBus)
    {
        _mqttService=mqttService;
        _mqttBus=mqttBus;
        _mqttBus.MessageReceived += async (topic, message) =>
        {
            await Task.Run(() =>
            {
                _viewModel.Messages.Add($"Topic: {topic}, Message: {message}");
            });
        };
    }
    // GET
    public IActionResult Index()
    {
        var model = new LogRequestAndResponseModel()
        {
            LogRequestModel = new LogRequestDto(),
            LogResponseModel = _viewModel
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
            _mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
            // Perform your logic here
            return RedirectToAction("Index"); // Or wherever you want to redirect
        }
        
        
        var model = new LogRequestAndResponseModel()
        {
            LogRequestModel = new LogRequestDto(),
            LogResponseModel = _viewModel
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
            await _mqttBus.SubscribeToTopic(topic);
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
        return Json(_viewModel.Messages);
    }
    public IActionResult Privacy()
    {
        return View();
    }
}