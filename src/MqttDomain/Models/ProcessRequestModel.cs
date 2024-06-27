using MqttDomain.Enums;

namespace MqttDomain.Models;

public class ProcessRequestModel
{
    public string Id { get; set; }
    public string SourceId { get; set; }
    public DateTime RequestDate { get; set; }
    public ProcessRequestDto ProcessRequestDto { get; set; } = new();
}

public class ProcessResponseModel
{
    
}

public class ProcessRequestDto
{
    public string TargetId { get; set; }
    public ActionType ActionType { get; set; }
    public bool IsAckRequired { get; set; }
    public ResponseType ResponseType { get; set; }
    public string ProcessNames { get; set; }
}