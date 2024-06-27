using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MqttHub.Bus;
using MqttHub.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace MqttHub.Extension;

public static class MqttServiceExtension
{
    public static IServiceCollection AddMqttService(this IServiceCollection services,Assembly assembly)
    {
        services.AddScoped<IMqttBus, MqttBus>(sp =>
        {
            var scopefactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new MqttBus(sp.GetService<IMediator>(), sp.GetService<IMqttClient>(), scopefactory,
                sp.GetService<IManagedMqttClient>());
        });
        services.AddSingleton<IManagedMqttClient>(opt =>
        {
            var factory = new MqttFactory();
            return factory.CreateManagedMqttClient();
        });
        services.AddSingleton<IMqttClient>(opt =>
        {
            var factory = new MqttFactory();
            return factory.CreateMqttClient();
        });
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped<IMqttService, MqttService>();
        services.AddScoped<IMqttBus, MqttBus>();
        return services;

    }
}