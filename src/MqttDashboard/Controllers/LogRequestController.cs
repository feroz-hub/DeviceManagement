
using Microsoft.AspNetCore.Mvc;
using MqttDomain.Models;
using MqttHub.Bus;
using MqttHub.Services;

namespace MqttDashboard.Controllers;

public class LogRequestController(IMqttService mqttService, IMqttBus mqttBus) : Controller
{
    private readonly LogResponseModel _viewModel = new ();

    // GET
    public IActionResult Index()
    {
        var model = new LogRequestAndResponseModel()
        {
            LogRequestModel = new LogRequestDto(),
            //LogResponseModel = _viewModel
        };
        return View(model);
    }

    private Task OnMessageReceivedAsync(string message, string topic)
    {
        return Task.Run(() =>
        {
            if((_viewModel.Messages!=null)) 
                _viewModel.Messages.Add($"Topic: {topic}, Message: {message}");
        });
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
            await Subscribe(logRequestModel.TargetId);
            mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
            // Perform your logic here
            return RedirectToAction("Index"); // Or wherever you want to redirect
        }
        
        
        var model = new LogRequestAndResponseModel()
        {
            LogRequestModel = new LogRequestDto(),
            //LogResponseModel = _viewModel
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

    // [HttpGet]
    // public IActionResult GetMessages()
    // {
    //     mqttBus.MessageReceived += async (message, topic) =>
    //     {
    //         await OnMessageReceivedAsync(message, topic);
    //     };
    //     var model = new LogRequestAndResponseModel()
    //     {
    //         LogRequestModel = new LogRequestDto(),
    //         //LogResponseModel = _viewModel
    //     };
    //     return PartialView("_MqttResponsePartial", model);
    // }
    public IActionResult Privacy()
    {
        return View();
    }
}