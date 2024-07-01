using Microsoft.AspNetCore.Mvc;
using MqttDashboard.Services;

namespace MqttDashboard;

public class MqttResponseViewComponent(IMessageReceiverService messageReceiverService):ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var messages = messageReceiverService.GetReceivedMessages();
        return View(messages);
    }
}