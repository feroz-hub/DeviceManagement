using Microsoft.EntityFrameworkCore;
using MqttDashboard.Data;
using MqttDashboard.Models;
using MqttDomain.Models;

namespace MqttDashboard.Infrastructure;

public class MqttClientRepository(ApplicationDbContext dbContext):IMqttClientRepository
{
    public async Task<List<string>> GetActiveClientIdsAsync()
    {
        return await dbContext.MqttClients
            .Where(client => client.Status)
            .Select(client => client.ClientId.ToString())
            .ToListAsync();
    }

    public async Task<List<MqttClientModel>> GetAllClientsAsync()
    {
        return await dbContext.MqttClients.ToListAsync();
    }

    public async Task<List<string>> GetAllClient()
    {
        return await dbContext.MqttClients
            .Select(client => client.DeviceName)
            .ToListAsync();
    }

    public async Task RegisterClientAsync(MqttClientModel mqttClientRegistration)
    {
        dbContext.MqttClients.Add(mqttClientRegistration);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateClientAsync(MqttClientModel clientRegistration)
    {
        dbContext.MqttClients.Update(clientRegistration);
        await dbContext.SaveChangesAsync();
    }
}