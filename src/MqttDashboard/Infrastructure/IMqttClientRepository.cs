using MqttDashboard.Models;
using MqttDomain.Models;

namespace MqttDashboard.Infrastructure;

public interface IMqttClientRepository
{
    Task<List<string>> GetActiveClientIdsAsync();
    Task<List<MqttClientModel>> GetAllClientsAsync();
    Task<List<string>> GetAllClient();
    Task RegisterClientAsync(MqttClientModel mqttClientRegistration);
    Task UpdateClientAsync(MqttClientModel clientRegistration);
}