using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Models;

namespace MqttDashboard.Controllers;

public class LogRequestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(LogRequestDto model)
    {
        if (ModelState.IsValid)
        {
            // Process the data
            // Convert the ViewModel to the DTO and save or use it as required
            var dto = new LogRequestDto
            {
                LogTypes = model.LogTypes,
                LogLevels = model.LogLevels,
                IsAckRequired = model.IsAckRequired,
                ActionType = model.ActionType,
                ResponseType = model.ResponseType,
                FromDate = model.FromDate,
                EndDate = model.EndDate
            };

            // Perform your logic here

            return RedirectToAction("Index"); // Or wherever you want to redirect
        }
        return View(model);
    }
}