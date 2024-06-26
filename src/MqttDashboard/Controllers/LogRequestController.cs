using Microsoft.AspNetCore.Mvc;

namespace MqttDashboard.Controllers;

public class LogRequestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}