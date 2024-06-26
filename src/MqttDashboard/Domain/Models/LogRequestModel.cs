using MqttDashboard.Domain.Enums;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace MqttDashboard.Models;

public class LogRequestModel
{
    public Guid RequestId { get; set; }
    public string TargetId { get; init; }=default!;
    public string SourceId { get; init; } = default!;
    public DateTime RequestDate { get; set; }
    public LogRequestDto LogRequestDto { get; init; } = default!;
}
public class LogRequestDto
{
    public List<LogType>  LogTypes { get; init; }=default!;
    public List<LogLevel> LogLevels { get; init; }=default!;
    public bool IsAckRequired { get; init; } 
    public ActionType ActionType { get; init; }
    public ResponseType ResponseType { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime EndDate { get; init; }
}