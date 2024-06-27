using MediatR;
using MqttDomain.Models;
using MqttHub.Commands;

namespace MqttHub.Services;

public class MqttService(IMediator mediator):IMqttService
{
    public async Task<bool> LogRequestPublishAsync(LogRequestModel logRequestModel)
    {
        var logRequestCommand = new LogRequestCommand(logRequestModel);
        await mediator.Send(logRequestCommand);
        return true;
    }
}