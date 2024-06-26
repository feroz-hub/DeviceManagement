using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Infrastructure;

namespace MqttDashboard;

public class RightSectionViewComponent(IMqttClientRepository mqttClientRepository):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var clients=await mqttClientRepository.GetAllClientsAsync();
        return View(clients);
    }
}