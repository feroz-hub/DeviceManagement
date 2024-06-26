namespace MqttDashboard.Domain.Enums;

public enum LogType
{
    SysLog,
    AppLog ,
    NetLog,
    ServiceLog,
    EventLog
}

public enum  LogLevel
{
    Info,
    Debug,
    Error,
    Warning,
    Fatal
}
public enum ResponseType
{
    MqttResponse,
    RestApiResponse
}
public enum ActionType
{
    LogRequest,
    PatchRequest,
    ProcessRequest,
    TpmConfiguration,
    TpmSealStorage,
    TpmNvStorage
}