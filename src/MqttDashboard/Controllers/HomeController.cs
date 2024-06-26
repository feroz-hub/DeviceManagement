using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Infrastructure;
using MqttDashboard.Models;

namespace MqttDashboard.Controllers;

public class HomeController(ILogger<HomeController> logger,IMqttClientRepository mqttClientRepository) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public async Task<IActionResult> Index()
    {
        var clients=await mqttClientRepository.GetAllClientsAsync();
        return View(clients);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
