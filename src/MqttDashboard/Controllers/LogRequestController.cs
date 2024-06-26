using Mapster;
using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Models;
using MqttDomain.Models;
using MqttHub.Services;

namespace MqttDashboard.Controllers;

public class LogRequestController (IMqttService mqttService): Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(LogRequestModel model)
    {
        if (!ModelState.IsValid) return View(model);
        // Process the data
        // Convert the ViewModel to the DTO and save or use it as required
        var dto = new LogRequestModel
        {
            RequestId = Guid.NewGuid(),
            TargetId = model.TargetId+"_Log",
            SourceId = "AdminClient",
            LogRequestDto = model.LogRequestDto,
            RequestDate = DateTime.Now
        };
        mqttService.LogRequestPublishAsync(dto).GetAwaiter().GetResult();
        // Perform your logic here

        return RedirectToAction("Index"); // Or wherever you want to redirect
    }

    public IActionResult Privacy()
    {
        return View();
    }
}