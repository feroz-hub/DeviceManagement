
using Microsoft.AspNetCore.Mvc;
using MqttDomain.Models;
using MqttHub.Bus;
using MqttHub.Services;

namespace MqttDashboard.Controllers;

public class LogRequestController : Controller
{
    private readonly IMqttBus _mqttBus;
    private readonly IMqttService _mqttService;
    public LogRequestController(IMqttBus mqttBus,IMqttService mqttService)
    {
        _mqttBus = mqttBus;
        _mqttService = mqttService;
        _mqttBus.MessageReceived += OnMessageReceived;
        
    }
    private readonly LogResponseModel _viewModel = new ();

    // GET
    public IActionResult Index()
    {
        return View(new LogRequestAndResponseModel());
    }

    private Task OnMessageReceived(string message, string topic)
    {
        _viewModel.Messages.Add($"{topic}: {message}");
        return Task.CompletedTask;
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
            _mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
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
    public async Task<IActionResult> SendLogRequest(LogRequestDto logRequestDto)
    {
        if (!ModelState.IsValid) return View("Index");
        var dto = new LogRequestModel
        {
            RequestId = Guid.NewGuid(),
            SourceId = "AdminClient",
            LogRequestDto = logRequestDto,
            RequestDate = DateTime.Now
        };
        await Subscribe(logRequestDto.TargetId);
        _mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
        // Perform your logic here
        return RedirectToAction("Index"); // Or wherever you want to redirect
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

    public IActionResult MessageReceived()
    {
        return PartialView("_MqttResponsePartial", _viewModel);
    }
}