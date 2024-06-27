using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Infrastructure;
using MqttDashboard.Models;
using MqttDomain.Models;

namespace MqttDashboard.Controllers;

public class MqttClientController(IMqttClientRepository mqttClientRepository) : Controller
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

    public async Task<IActionResult> Client()
    {
        var clients=await mqttClientRepository.GetAllClientsAsync();
        return View(clients);
    }
}