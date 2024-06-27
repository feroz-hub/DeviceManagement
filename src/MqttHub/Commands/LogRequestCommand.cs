using MediatR;
using MqttDomain.Models;


namespace MqttHub.Commands;

public class LogRequestCommand(LogRequestModel logRequestModel):IRequest<bool>
{
    public LogRequestModel LogRequestModel => new()
    {
        RequestId = logRequestModel.RequestId,
        SourceId = logRequestModel.SourceId,
        LogRequestDto = logRequestModel.LogRequestDto,
        RequestDate = logRequestModel.RequestDate
    };
}