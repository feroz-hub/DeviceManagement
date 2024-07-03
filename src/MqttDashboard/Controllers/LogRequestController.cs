
using Microsoft.AspNetCore.Mvc;
using MqttDomain.Models;
using MqttHub.Bus;
using MqttHub.Services;

namespace MqttDashboard.Controllers;

public class LogRequestController(IMqttBus mqttBus, IMqttService mqttService,MessageService messageService) : Controller
{
    
    public IActionResult Index()
    {
        //_mqttBus.MessageReceived += OnMessageReceived;
        var viewModel = new LogRequestAndResponseModel
        {
            LogResponseModel = new LogResponseModel
            {
                Messages = mqttBus.GetMessages().ToList()
            }
        };
        return View(viewModel);
        
    }

    private  Task OnMessageReceived(string message, string topic)
    {
        messageService.Messages.Add($"{topic}: {message}");
        return Task.CompletedTask;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LogRequestDto logRequestModel)
    {
        if (ModelState.IsValid) return RedirectToAction("Index"); // Or wherever you want to redirect
        var dto = new LogRequestModel
        {
            RequestId = Guid.NewGuid(),
            SourceId = "AdminClient",
            LogRequestDto = logRequestModel,
            RequestDate = DateTime.Now
        };
        //await Subscribe(logRequestModel.TargetId);
        mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
        return RedirectToAction("Index"); // Or wherever you want to redirect
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
        mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
        await Subscribe(logRequestDto.TargetId);
        // Perform your logic here
        return RedirectToAction("Index"); // Or wherever you want to redirect
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

    public IActionResult GetMessagesPartial()
    {
        // _mqttService.MessageReceived += OnMessageReceived;

      
        var logResponseModel = new LogResponseModel
        {
            Messages = mqttBus.GetMessages().ToList()
            //Messages=messageService.Messages
        };
        return PartialView("_MqttResponsePartial", logResponseModel);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    // public IActionResult MessageReceived()
    // {
    //     lock (_lock)
    //     {
    //         return PartialView("_MqttResponsePartial", _viewModel);
    //     }
    // }
}