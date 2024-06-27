using Microsoft.AspNetCore.Mvc;

namespace MqttDashboard.Controllers;

public class ProcessRequestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}