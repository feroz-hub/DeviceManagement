using MqttDomain.Enums;

namespace MqttDomain.Models;

public class LogRequestModel
{
    public Guid RequestId { get; set; }
    public string SourceId { get; init; } = default!;
  
    public DateTime RequestDate { get; set; }
    public LogRequestDto LogRequestDto { get; init; } = default!;
}
public class LogRequestDto
{
    public string TargetId { get; init; }=default!;
    public List<LogType>  LogTypes { get; init; }= [];
    public List<LogLevel> LogLevels { get; init; } = [];
    public bool IsAckRequired { get; init; } 
    public ActionType ActionType { get; init; }
    public ResponseType ResponseType { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime EndDate { get; init; }
}
public class LogResponseModel
{
    public List<string> Messages { get; set; }
}
public class LogRequestAndResponseModel
{
    public LogRequestDto LogRequestModel { get; set; }
    public  LogResponseModel  LogResponseModel{ get; set; }
}