using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Models;

namespace MqttDashboard.Controllers;

public class MqttClientController : Controller
{
    // GET
    public IActionResult Index()
    {
        
        return View();
    }

    [HttpPost]
    public IActionResult Index(MqttClientRegistrationDto model)
    {
        if (ModelState.IsValid)
        {
            // Save the model or perform other actions
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }
    
}