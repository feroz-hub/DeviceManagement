using System.ComponentModel.DataAnnotations;

namespace MqttDashboard.Models;

public class MqttClientModel
{
    [Key]
    public Guid ClientId { get; init; }
    public string DeviceName { get; init; } = default!;
    public DateTime Created { get; init; }
    public List<string> SubscribedTopics { get; init; }
    public DateTime LastAccessed { get; init; }
    public bool Status { get; init; }
    public int KeepAlive { get; init; }
    public string IpAddress { get; init; } = default!;
}

public class MqttClientRegistrationDto
{
    public string DeviceName { get; set; }
    public int KeepAlive { get; set; }
    public List<string> SubscribedTopics { get; set; }
}