using MediatR;
using MqttHub.Bus;
using MqttHub.Commands;

namespace MqttHub.Handlers;

public class LogRequestHandler(IMqttBus mqttBus):IRequestHandler<LogRequestCommand,bool>
{
    public async Task<bool> Handle(LogRequestCommand request, CancellationToken cancellationToken)
    {
        await mqttBus.ManagedMqttPublish(request.LogRequestModel, request.LogRequestModel.LogRequestDto.TargetId);
        return await Task.FromResult(true);
    }
}