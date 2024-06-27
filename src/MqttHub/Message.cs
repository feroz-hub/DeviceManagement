using MediatR;

namespace MqttHub;

public class Message:IRequest<bool>
{
    public string MessageType { get;protected set; }

    protected Message()
    {
        MessageType = GetType().Name;
    }  
}