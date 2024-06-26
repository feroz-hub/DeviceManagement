using MqttDomain.Models;

namespace MqttHub.Services;

public interface IMqttService
{
    public Task<bool> LogRequestPublishAsync(LogRequestModel logRequestModel);
}